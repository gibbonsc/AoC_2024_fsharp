open System.IO

open Day1part2.Parse
open Day1part2.Columns
open Day1part2.SimilarityScores

[<EntryPoint>]
let main (args : string array) : int =
    // let inputPath = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let Solution =
        File.ReadLines
        >> tryParseLines
        >> toColumns
    // let result =
    //     Solution inputPath
    // let formatted : string =
    //     let (left: int list),(right: int list) = result
    //     let formatList (fL: int list) : string =
    //         fL
    //         |> List.map string
    //         |> String.concat "; "
    //     $"[{formatList left}] [{formatList right}]"
    // printfn $"{formatted}"

    //    >> counts
        >> scores
    // let result =
    //     Solution inputPath
    // let formatted : string =
    //     result
    //     |> List.map string
    //     |> String.concat "; "
    // printfn $"{formatted}"
        >> List.sum
    let result =
        Solution inputPath
    printfn $"{result}"

    0
