open System
open System.IO

[<EntryPoint>]
let main(args: string array) : int =
    // printfn $"{args.Length}"
    let inputPath =
        match args.Length with
        // | 0 -> "example.txt"
        | 0 -> "input.txt"
        | _ -> args.[0]
    // printfn $"{inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath
    // inputLines |> Array.iter (printfn "%s")

    let parseLine (line: string): (int64 * int64 list * string) = //(int * int list ) =
        let parts = line.Split(':')
        let testValue = int64(parts[0])
        let operandStrings = parts[1].Trim().Split(' ')
        let operandList =
            operandStrings
            |> Array.map (fun s -> int64 s)
            |> Array.toList
        // printfn $"{testValue}: {operandList}"
        testValue, operandList, ""

    let parsedLines =
        inputLines
        |> Array.map parseLine
        |> Array.toList
    // printfn $"{parsedLines}"

    let tryNextOperator
        (target: int64)
        (operand: int64)
        : (string option * int64) =

        let q, r = Math.DivRem(target, operand)
        if r = 0 then
            Some "*", q
        elif (target - operand) >= 0 then
            Some "+", (target - operand)
        else
            None, target

    let rec tryOperators
        (target: int64)
        (operands: int64 list)
        (operators: string)
        : (int64 * int64 list * string) =

        match operands with
        | head :: tail when (List.length tail > 0) ->
            let q, r = Math.DivRem(target, head)
            let d = target - head
            // is target divisible by head?
            if r = 0 then
                // divide and try remaining operands
                let target', operands', operators' =
                    tryOperators q tail ("*" + operators)
                // did that succeed?
                if target' = 0 then
                    target', operands', operators'
                else
                    // let target'', operands'', operators'' =
                        tryOperators d tail ("+" + operators)
                    // if target'' = 0 then
                    //    target'', operands'', operators''
                    //else
                    //    -1, tail, ("?" + operators)
            elif d > 0 then
                tryOperators d tail ("+" + operators)
            else
                -1, tail, ("?" + operators)
        | _ ->
            if operands[0] = target then
                0, operands, operators
            else
                target, operands, operators

    // let tried =
    //     parsedLines
    //     |> List.map (
    //         fun p ->
    //         let t,n, ops = p
    //         tryOperators t (List.rev n) ops
    //         )

    // tried
    // |> List.map (
    //     fun p ->
    //     let t, n, ops = p
    //     printfn $"{t} {n} {ops}"
    // ) |> ignore

    let canCalibrate (parsedLine: int64 * int64 list * string): bool =
        let t, n, ops = parsedLine
        let t', _, _ =
            tryOperators t (List.rev n) ops
        t' = 0

    let filtered: (int64 * int64 list * string) list =
        List.filter canCalibrate parsedLines

    // filtered
    // |> List.map (
    //     fun p ->
    //     let t, n, ops = p
    //     printfn $"{t} {n} {ops}"
    // ) |> ignore

    let calibrationTargets =
        filtered
        |> List.map (
            fun p ->
            let t, _, _ = p
            t
        )

    let result = List.sum calibrationTargets
    printfn $"{result}"

    0
