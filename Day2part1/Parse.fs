module Parse

open System

let tryParseLine (line:string) : int list =
    line.Split(
        [| ' '; '\t' |],
        StringSplitOptions.RemoveEmptyEntries
    )
    |> Array.toList
    |> List.map Int32.Parse

let tryParseLines (lines: string seq) : int list list =
    lines
    |> Seq.map tryParseLine
    |> Seq.toList
