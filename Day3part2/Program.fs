open System.Text.RegularExpressions
open System.IO

let evalMul (mulInstruction : string) : int =
    let stringLength = mulInstruction.Length
    let intArgs = mulInstruction.[4..(stringLength - 2)]
    let ints = intArgs.Split [| ',' |]
    if ints.Length <> 2 then
        // return 0 if more or fewer than 2 arguments
        0
    else
        (int ints[0]) * (int ints[1])

let processDontDo (s: string): string list =
    let dontDoPattern = "don't\(\).*?(?:do\(\)|$)" // ".*?" isn't greedy
    Regex.Split(s, dontDoPattern, RegexOptions.Singleline)
    |> Array.toList

let matchGroup1Value (m: Match): string =
    m.Groups.[1].Value

let isolateMulTokens (s: string): string list =
    let mulPattern = @"(mul\(\d{1,3},\d{1,3}\))"
    Regex.Matches(s, mulPattern)
    |> Seq.cast<Match>
    |> Seq.map matchGroup1Value
    |> Seq.toList

[<EntryPoint>]
let main(args: string array): int =
    // let inputPath = "example.txt"
    // let inputPath = "example2.txt"
    let inputPath = "input.txt"
    printfn $"{inputPath}"

    let inputText: string = 
        File.ReadAllText inputPath

    // printfn $"{inputText}"

    let filteredStrings: string list =
        inputText
        |> processDontDo

    // filteredStrings |> List.iter (printfn "Element: %s")

    let mulTokens: string list =
        filteredStrings
        |> List.collect isolateMulTokens

    // mulTokens |> List.iter (printf " %s")
    // printfn("")

    let products : int list =
        mulTokens
        |> List.map evalMul

    let result : int = List.sum products

    printfn $"{result}"

    0
