namespace Day1part2
open System

module Parse =
    let tryParseInt (s: string) : int option =
        match Int32.TryParse s with
        | true, v -> Some v
        | _ -> None

    let tryParsePair (line:string) : (int * int) option =
        let tokens: int option array =
            line.Split([| ' '; '\t' |],
                StringSplitOptions.RemoveEmptyEntries)
            |> Array.map tryParseInt
        match tokens with
        | [| Some a; Some b|] -> Some (a,b)
        | _ -> None
    
    let tryParseLines (lines: string seq) : (int * int) array =
        lines
        |> Seq.map tryParsePair
        |> Seq.choose id
        |> Array.ofSeq
