module Parse

open System

let tryParseLine (line:string) : int array =
    line.Split(
        [| ' '; '\t' |],
        StringSplitOptions.RemoveEmptyEntries
    )
    |> Array.map Int32.Parse

let tryParseLines (lines: string seq) : int array list =
    lines
    |> Seq.map tryParseLine
    |> Seq.toList
