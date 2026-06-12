open System
open System.IO

[<EntryPoint>]
let main(args: string array) : int =
    // let DEBUG = true // false to disable debug log tracing
    // if DEBUG then printfn $"argc: {args.Length}"
    let inputPath =
        match args.Length with
        // | 0 -> "example.txt"
        | 0 -> "input.txt"
        | _ -> args.[0]
    // if DEBUG then printfn $"inputPath: {inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath
    // if DEBUG then inputLines |> Array.iter (printfn "%s")

    let parseLine (line: string): (int64 * int64 list) =
        let parts = line.Split(':')
        let testValue = int64(parts[0])
        let operandStrings = parts[1].Trim().Split(' ')
        let operandList =
            operandStrings
            |> Array.map (fun s -> int64 s)
            |> Array.toList
        // if DEBUG then printfn $"{testValue}: {operandList}"
        testValue, operandList

    let parsedLines =
        inputLines
        |> Array.map parseLine
        |> Array.toList
    // if DEBUG then printfn $"{parsedLines}"

    let tryOpsSucceeded ((t, _): (int64 * string)): bool =
        t = 0L

    let rec tryOperations
        (operands: int64 list)
        (state: (int64 * string) option)
        : (int64 * string) option =

        let target, operators =
            state |> Option.defaultValue (-1, "?")

        match operands with
        | [] -> failwith "invalid input: empty operand list"
        | [tail] ->
            if tail = target then
                Some (0, operators)
            else
                None
        | head :: tail ->
            // Gemini helped me DRY refactor this pattern match code
            //   to use a tryOp helper function
            let tryOp tryTarget trySymbol: (int64 * string) option =
                tryOperations tail (Some (tryTarget, (trySymbol + operators)))
                |> Option.filter tryOpsSucceeded

            let trySplat (): (int64 * string) option =
                let q, r = Math.DivRem(target, head)
                if r = 0L then
                    (* invert multiplication by head*)
                    tryOp q "*"
                else
                    None

            let tryConcat (): (int64 * string) option =
                let targetAsString = string target
                let targetDigits = String.length targetAsString
                let headAsString = string head
                let headDigits = String.length headAsString
                if targetDigits > headDigits &&
                    targetAsString.EndsWith(headAsString) then
                    (* invert concatenation with head *)
                    // if DEBUG then
                    //     printfn $"    -- try invert concat w/ {target}, {head}"
                    let s = targetAsString.Substring(
                        0,
                        targetDigits - headDigits
                    )
                    let i = int64 s
                    tryOp i "|"
                else
                    None

            let  tryPlus (): (int64 * string) option =
                let d = target - head
                if d > 0L then
                    (* invert addition of head *)
                    tryOp d "+"
                else
                    None

            () |> trySplat
            |> Option.orElseWith tryConcat
            |> Option.orElseWith tryPlus

    let canCalibrate (parsedLine: int64 * int64 list): bool =
        let t, n = parsedLine
        // if DEBUG then printfn $"Calibrating {t}: {n}"
        let attempt=
            tryOperations (List.rev n) (Some (t, ""))
        // if DEBUG then
        //     match attempt with
        //     | Some p -> printfn $"  {snd p}"
        //     | None -> printfn "  ?"
        Option.exists tryOpsSucceeded attempt

    let filtered: (int64 * int64 list) list =
        List.filter canCalibrate parsedLines

    // if DEBUG then filtered |> List.map (fun p -> printfn $"{fst p} {snd p}") |> ignore

    let calibrationTargets =
        filtered
        |> List.map (
            fun p ->
            let t, _ = p
            t
        )

    let result = List.sum calibrationTargets
    printfn $"{result}"

    0
