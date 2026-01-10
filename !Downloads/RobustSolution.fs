// RobustSolution.fs
open System
open System.IO
open System.Text.RegularExpressions

let tryParseInt (s: string) : int option =
    match Int32.TryParse s with
    | true, v -> Some v
    | _ -> None

let tryParsePair (line: string) : Result<(int * int), string> =
    let tokens = Regex.Split(line, "\\s+") |> Array.filter (fun s -> s <> "")
    match tokens |> Array.map tryParseInt with
    | [| Some a; Some b |] -> Ok (a, b)
    | _ -> Error (sprintf "Line '%s' does not contain exactly two integers" line)

let parseFile (path: string) : Result<(int * int) list, string> =
    File.ReadLines(path)
    |> Seq.map tryParsePair
    |> Seq.fold (fun acc r ->
        match acc, r with
        | Ok xs, Ok x -> Ok (x :: xs)
        | Error e, _ -> Error e
        | Ok _, Error e -> Error e) (Ok [])
    |> Result.map List.rev

let sumMagnitudeDiffPairs (pairs: (int * int) list) : int =
    let c1, c2 = List.unzip pairs
    let s1, s2 = List.sort c1, List.sort c2
    List.map2 (fun x y -> abs (x - y)) s1 s2 |> List.sum

[<EntryPoint>]
let main argv =
    let path = if argv.Length > 0 then argv[0] else "input.txt"
    match parseFile path with
    | Ok pairs ->
        let result = sumMagnitudeDiffPairs pairs
        printfn "%d" result
        0
    | Error msg ->
        eprintfn "%s" msg
        1
