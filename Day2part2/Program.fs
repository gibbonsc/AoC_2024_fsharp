module Program

open System.IO
open Parse
open Logic

[<EntryPoint>]
let main(args: string array) : int =
    // let inputPath = "example.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let inputLines: string seq = 
        File.ReadLines inputPath

    let inputCollection: int array list =
        tryParseLines inputLines

    let processLines (inputCollection : int array list) : bool list =
        inputCollection
        // |> List.map (fun (p : int array) -> (isGradual3 p && isMonotone p))
        |> List.map (fun (p : int array) -> isDamp1Safe p)

    let result : int = 
        processLines inputCollection
        |> countTrue

    printfn $"{result}"

    0
