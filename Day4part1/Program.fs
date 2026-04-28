open System.IO

[<EntryPoint>]
let main(args : string array) : int =
    // let inputPath = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath
    // inputLines |> Array.iter (printfn "Element: %s")

    let rows = inputLines.Length
    let cols = inputLines.[0].Length

    let checkHoriz (r: int) (c: int) : int =
        if c > cols-4 then
            0
        elif
            inputLines.[r].Substring(c,1) = "X" &&
            inputLines.[r].Substring(c+1,1) = "M" &&
            inputLines.[r].Substring(c+2,1) = "A" &&
            inputLines.[r].Substring(c+3,1) = "S"
        then
            // printfn $"found at {r},{c} horiz w-e"
            1
        elif
            inputLines.[r].Substring(c,1) = "S" &&
            inputLines.[r].Substring(c+1,1) = "A" &&
            inputLines.[r].Substring(c+2,1) = "M" &&
            inputLines.[r].Substring(c+3,1) = "X"
        then
            // printfn $"found at {r},{c} horiz e-w"
            1
        else
            0
    let checkVert (r: int) (c: int) : int =
        if r > rows-4 then
            0
        elif
            inputLines.[r].Substring(c,1) = "X" &&
            inputLines.[r+1].Substring(c,1) = "M" &&
            inputLines.[r+2].Substring(c,1) = "A" &&
            inputLines.[r+3].Substring(c,1) = "S"
        then
            // printfn $"found at {r},{c} vert n-s"
            1
        elif
            inputLines.[r].Substring(c,1) = "S" &&
            inputLines.[r+1].Substring(c,1) = "A" &&
            inputLines.[r+2].Substring(c,1) = "M" &&
            inputLines.[r+3].Substring(c,1) = "X"
        then
            // printfn $"found at {r},{c} vert s-n"
            1
        else
            0
    let checkSlash (r: int) (c: int) : int =
        if r > rows-4 || c > cols-4 then
            0
        elif
            inputLines.[r+3].Substring(c,1) = "X" &&
            inputLines.[r+2].Substring(c+1,1) = "M" &&
            inputLines.[r+1].Substring(c+2,1) = "A" &&
            inputLines.[r].Substring(c+3,1) = "S"
        then
            // printfn $"found at {r},{c} slash sw-ne"
            1
        elif
            inputLines.[r+3].Substring(c,1) = "S" &&
            inputLines.[r+2].Substring(c+1,1) = "A" &&
            inputLines.[r+1].Substring(c+2,1) = "M" &&
            inputLines.[r].Substring(c+3,1) = "X"
        then
            // printfn $"found at {r},{c} slash ne-sw"
            1
        else
            0
    let checkBackSlash (r: int) (c: int) : int =
        if r > rows-4 || c > cols-4 then
            0
        elif
            inputLines.[r].Substring(c,1) = "X" &&
            inputLines.[r+1].Substring(c+1,1) = "M" &&
            inputLines.[r+2].Substring(c+2,1) = "A" &&
            inputLines.[r+3].Substring(c+3,1) = "S"
        then
            // printfn $"found at {r},{c} backslash nw-se"
            1
        elif
            inputLines.[r].Substring(c,1) = "S" &&
            inputLines.[r+1].Substring(c+1,1) = "A" &&
            inputLines.[r+2].Substring(c+2,1) = "M" &&
            inputLines.[r+3].Substring(c+3,1) = "X"
        then
            // printfn $"found at {r},{c} backslash se-nw"
            1
        else
            0

    let tryRows = [ 0 .. (rows - 1) ]
    let tryCols = [ 0 .. (cols - 1) ]

    let pairs = List.allPairs tryRows tryCols

    let matchesHoriz : int list = 
        pairs
        |> List.map (fun ( p: (int*int)) -> checkHoriz (fst p) (snd p) )
    let matchesVert : int list = 
        pairs
        |> List.map (fun ( p: (int*int)) -> checkVert (fst p) (snd p) )
    let matchesSlash : int list = 
        pairs
        |> List.map (fun ( p: (int*int)) -> checkSlash (fst p) (snd p) )
    let matchesBackSlash : int list = 
        pairs
        |> List.map (fun ( p: (int*int)) -> checkBackSlash (fst p) (snd p) )

    let matchesAll = List.concat (seq {matchesHoriz; matchesVert; matchesSlash; matchesBackSlash})
    let result =
        matchesAll
        |> List.sum

    printfn $"{result}"

    0
