open System.IO

open Day1part1.Parse
open Day1part1.Columns
open Day1part1.Differences

[<EntryPoint>]
let main (args : string array) : int =
    // let inputPath: string = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"
    // let inputLines : string seq =
    //    File.ReadLines(inputPath)

    // let pairs: (int * int) array =
    //    tryParseLines inputLines

    // let formatted: string =
    //     pairs
    //     |> Array.map (fun (a, b) -> $"({a}, {b})")
    //     |> String.concat "; "
    // printfn $"[{formatted}]"

    // let cols : int list * int list =
    //     toSortedColumns pairs
    // let formatted: string =
    //     let (left: int list),(right: int list) = cols
    //     let formatList (fL: int list) : string =
    //         fL
    //         |> List.map string
    //         |> String.concat "; "
    //     $"[{formatList left}] [{formatList right}]"
    // printfn $"{formatted}"

    let Solution = 
        File.ReadLines
        >> tryParseLines
        >> toSortedColumns
        >> sumDiffs
    let result : int =
        Solution inputPath

    printfn $"{result}"
    0
