// Run.fs
module Run =
    open GetInput
    open Columns

    let fileName = "example.txt"

    // Read lines as immutable list<string>
    let lines : string list = readLinesList fileName

    // Tokenize by whitespace and parse to ints
    let rows : int list list = lines |> List.map splitByWhitespace |> List.map parseIntTokens

    // Convert to (int * int) tuples (expects exactly two ints per row)
    let toTuples (rows: int list list) : (int * int) list =
        rows |> List.map (function | [a; b] -> (a, b) | _ -> failwith "Expected exactly two ints per line")

    let pairs = toTuples rows
    let c1, c2 = Columns.columns pairs
    let c1sorted, c2sorted = Columns.sortedColumns pairs

    printfn "c1: %A" c1
    printfn "c2: %A" c2
    printfn "c1sorted: %A" c1sorted
    printfn "c2sorted: %A" c2sorted
