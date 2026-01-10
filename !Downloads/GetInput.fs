// GetInput.fs
module GetInput =
    open System
    open System.IO
    open System.Text.RegularExpressions

    /// Read entire file into an immutable list<string>
    let readLinesList (path: string) : string list =
        File.ReadLines(path) |> Seq.toList

    /// Split by one-or-more whitespace (regex \s+), return list<string>
    let splitByWhitespace (line: string) : string list =
        Regex.Split(line, "\\s+")
        |> Array.filter (fun s -> s <> "")
        |> Array.toList

    /// Parse tokens to ints, skipping non-ints
    let parseIntTokens (tokens: string list) : int list =
        tokens
        |> List.choose (fun s -> match Int32.TryParse s with | true, v -> Some v | _ -> None)
