open Day3part1.Lib.Parse
open System.IO

[<EntryPoint>]
let main(args : string array) : int =
    // let inputPath = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let inputText : string = 
        File.ReadAllText inputPath

    let mulTokens : string list =
        scan inputText

    let products : int list =
        mulTokens
        |> List.map evalMul

    let result : int = List.sum products

    printfn $"{result}"
    0
