module Program

// open Expecto
// let allTests =
//     testList "All tests" [
//         LogicTests.allLogicTests
//         ParseTests.allParseTests
//     ]
//[<EntryPoint>]
// let main argv =
//     runTestsWithCLIArgs [] argv allTests

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

    let inputCollection: int list list =
        tryParseLines inputLines

    let processLines (inputCollection : int list list) : bool list =
        inputCollection
        |> List.map (fun (p : int list) -> (isGradual3 p && isMonotone p))
    
    let result : int = 
        processLines inputCollection
        |> countTrue

    printfn $"{result}"

    0
