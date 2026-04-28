open System.IO

[<EntryPoint>]
let main(args : string array) : int =
    // let inputPath = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath
    // inputLines |> Array.iter (printfn "Line: %s")

    let rows = inputLines.Length
    let cols = inputLines.[0].Length

    let checkMas (r: int) (c: int) : int =
        if r > rows-3 || c > cols-3 then
            0
        elif inputLines.[r+1].Substring(c+1,1) <> "A" then
            0
        elif
            inputLines.[r].Substring(c,1) = "M" &&
            inputLines.[r+2].Substring(c,1) = "M" &&
            inputLines.[r].Substring(c+2,1) = "S" &&
            inputLines.[r+2].Substring(c+2,1) = "S"
        then
            // printfn $"found at {r},{c} w-e"
            1
        elif
            inputLines.[r].Substring(c,1) = "S" &&
            inputLines.[r+2].Substring(c,1) = "S" &&
            inputLines.[r].Substring(c+2,1) = "M" &&
            inputLines.[r+2].Substring(c+2,1) = "M"
        then
            // printfn $"found at {r},{c} e-w"
            1
        elif
            inputLines.[r].Substring(c,1) = "M" &&
            inputLines.[r].Substring(c+2,1) = "M" &&
            inputLines.[r+2].Substring(c,1) = "S" &&
            inputLines.[r+2].Substring(c+2,1) = "S"
        then
            // printfn $"found at {r},{c} n-s"
            1
        elif
            inputLines.[r].Substring(c,1) = "S" &&
            inputLines.[r].Substring(c+2,1) = "S" &&
            inputLines.[r+2].Substring(c,1) = "M" &&
            inputLines.[r+2].Substring(c+2,1) = "M"
        then
            // printfn $"found at {r},{c} s-n"
            1
        else
            0

    let tryRows = [ 0 .. (rows - 3) ]
    let tryCols = [ 0 .. (cols - 3) ]

    let pairs = List.allPairs tryRows tryCols

    let matchesAll : int list = 
        pairs
        |> List.map (fun ( p: (int*int)) -> checkMas (fst p) (snd p) )

    let result =
        matchesAll
        |> List.sum

    printfn $"{result}"

    0
