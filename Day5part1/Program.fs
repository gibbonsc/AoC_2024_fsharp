open System.IO

[<EntryPoint>]
let main(args: string array) : int =
    // printfn $"{args.Length}"
    let inputPath =
        match args.Length with
        | 0 -> "input.txt"
        | _ -> args.[0]
    // printfn $"{inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath

    // inputLines |> Array.iter (printfn "%s")

    // how to distinguish a rule
    let isOrderRule (line: string): bool =
        line.Contains('|')

    // how to discard or collect a rule
    let decideBacker
        (line: string)
        (ins: (string list * string list))
        : (string list * string list) =
        if line.Length = 0 then
            ins
        elif isOrderRule line then
            ((line :: fst ins), (snd ins))
        else
            ((fst ins), (line :: snd ins))

    // partition the input into page order rules and production order rules
    let (pageOrderRules, pagesToProduce): (string list * string list) =
        Array.foldBack decideBacker inputLines ([], [])

    // printfn "order rules:"
    // pageOrderRules |> List.iter (printfn "%s")
    // printfn "productions:"
    // pagesToProduce |> List.iter (printfn "%s")

    // how to parse a rule's integer
    let rec scanUInt
        (chars: char list)
        (digits: char list)
        : (uint * char list) =
        match chars with
        | head :: tail ->
            match head with
            | c when '0' <= c && c <= '9' ->
                scanUInt tail (head :: digits)
            | _ ->
                ((uint (List.rev digits |> List.toArray |> System.String)), tail)
        | [] ->
            match digits with
            | [] ->
                (0u, [])
            | _ ->
                ((uint (List.rev digits |> List.toArray |> System.String)), [])

    // how to parse an order rule
    let parseOrderRule (s: string) : (uint * uint) =
        let (sChars: char list) = List.ofSeq s
        let (firstUInt, after) = scanUInt sChars []
        let (secondUInt, _) = scanUInt after []
        (firstUInt, secondUInt)

    // parse page order rules
    let (orderRules: (uint * uint) seq) =
        pageOrderRules
        |> List.map parseOrderRule
        |> List.toSeq

    // orderRules |> Seq.iter (fun x -> printfn "%u %u" (fst x) (snd x))
    // printfn "%u order rules" (Seq.length orderRules)

    // how to parse production request rule
    let parseProdRule (s: string) : (uint list) =
        let (sChars: char list) = List.ofSeq s
        let rec parseProds (chars: char list) (nums: uint list) : uint list =
            let (num: uint, after: char list) = scanUInt chars []
            match after with
            | [] ->
                List.rev (num :: nums)
            | _ ->
                parseProds after (num :: nums)
        parseProds sChars []

    let (prodUpdateLists: uint list list) =
        pagesToProduce
        |> List.map parseProdRule

    // let sidePrintf (nums: uint list) =
    //     nums
    //     |> List.iter (printf " %u")
    //     printfn ""
    // prodUpdateLists
    // |> List.iter sidePrintf

    (* Parsing complete. Now to analyze the requests against the order rules...
      assumptions: (based on example.txt and input.txt)
      - there are n pages referenced in the input
      - rules for all 1/2 * n * (n+1) orderes are provided
      - every request for pages has an odd page count
    *)
    let getProdUpdatePairs (p: uint list): (uint * uint) list =
        p
        |> List.pairwise
    let followsAnOrderRule (consecutivePair: uint * uint) : bool =
        Seq.exists (fun x -> x = consecutivePair) orderRules
    let allProdUpdatesFollowOrderRules (puList: uint list) : bool =
        puList
        |> getProdUpdatePairs
        |> List.forall followsAnOrderRule
    let (validProdUpdateLists: uint list list) =
        prodUpdateLists
        |> List.filter allProdUpdatesFollowOrderRules

    // let sidePrintf (nums: uint list) =
    //     nums
    //     |> List.iter (printf " %u")
    //     printfn ""
    // validProdUpdateLists
    // |> List.iter sidePrintf

    let getMiddle (el: uint list): uint =
        let index = (List.length el - 1) / 2
        el.[index]
    let (middles: uint list) =
        validProdUpdateLists
        |> List.map (fun x -> getMiddle x)
    let sumOfMiddles: uint =
        List.sum middles
    printf "%u" sumOfMiddles

    0
