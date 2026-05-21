open System.IO
type Direction = Up | Right | Down | Left

[<EntryPoint>]
let main(args: string array) : int =
    // printfn $"{args.Length}"
    let inputPath =
        match args.Length with
        | 0 -> "input.txt"
        | _ -> args.[0]
    // printfn $"{inputPath}"

    let inputLines : string array = 
        File.ReadAllLines inputPath
    // inputLines |> Array.iter (printfn "%s")

    let mapRowCount = Array.length inputLines
    let mapColCount = Seq.length inputLines.[0]
    // printfn $"total rows, cols: {mapRowCount}, {mapColCount}"

    let (initialRow: int option) =
        Array.tryFindIndex (fun (x: string) -> x.Contains('^')) inputLines
    if initialRow = None then
        failwith "Couldn't find '^' in input"
    let areaMap: char array array =
        inputLines
        |> Array.map (fun x -> Seq.toArray x)
    let (initialCol: int option) =
        Array.tryFindIndex
            (fun (x: char) -> x = '^')
            areaMap.[initialRow.Value]
    // if initialCol = None then
    //     failwith $"Couldn't find '^' in initial row index {initialRow.Value}"
    // printfn $"initial row, col: {initialRow.Value}, {initialCol.Value}"

    (* finished modeling input, now trace guard's path *)
    let sidePrintAreaMap () =
        System.Console.Clear()
        // System.Console.CursorLeft <- 0
        // System.Console.CursorTop <- 0
        System.Console.SetCursorPosition(0,0)
        let frameString =
            areaMap
            |> Array.map (fun r -> System.String(r))
            |> String.concat "\n"
        printf "%s" frameString
        // let sidePrintf (row: char array) =
        //     row
        //     |> Array.iter (fun c -> printf $"{c}")
        //     printfn ""
        // areaMap
        // |> Array.iter sidePrintf
        // printfn ""
        System.Console.ReadLine() |> ignore

    let mutable (guardDirection: Direction) = Up
    let mutable (guardRow: int) = initialRow.Value
    let mutable (guardCol: int) = initialCol.Value
    let mutable (cellValue: char) = areaMap.[guardRow].[guardCol]

    while
        0 <= guardRow && guardRow < mapRowCount &&
        0 <= guardCol && guardCol < mapColCount do
        if guardDirection = Up then
            if guardRow = 0 then
                areaMap.[guardRow].[guardCol] <- 'X'
                guardRow <- guardRow - 1
            else
                cellValue <- areaMap.[guardRow - 1].[guardCol]
                if cellValue = '#' then
                    guardDirection <- Right
                else
                    areaMap.[guardRow].[guardCol] <- 'X'
                    guardRow <- guardRow - 1
        elif guardDirection = Right then
            if guardCol = mapRowCount - 1 then
                areaMap.[guardRow].[guardCol] <- 'X'
                guardCol <- guardCol + 1
            else
                cellValue <- areaMap.[guardRow].[guardCol + 1]
                if cellValue = '#' then
                    guardDirection <- Down
                else
                    areaMap.[guardRow].[guardCol] <- 'X'
                    guardCol <- guardCol + 1
        elif guardDirection = Down then
            if guardRow = mapRowCount - 1 then
                areaMap.[guardRow].[guardCol] <- 'X'
                guardRow <- guardRow + 1
            else
                cellValue <- areaMap.[guardRow + 1].[guardCol]
                if cellValue = '#' then
                    guardDirection <- Left
                else
                    areaMap.[guardRow].[guardCol] <- 'X'
                    guardRow <- guardRow + 1
        elif guardDirection = Left then
            if guardCol = 0 then
                areaMap.[guardRow].[guardCol] <- 'X'
                guardCol <- guardRow - 1
            else
                cellValue <- areaMap.[guardRow].[guardCol - 1]
                if cellValue = '#' then
                    guardDirection <- Up
                else
                    areaMap.[guardRow].[guardCol] <- 'X'
                    guardCol <- guardCol - 1
        sidePrintAreaMap ()

    (*count visited areaMap cells*)
    let (visitedCoordinates: int) =
        areaMap
        |> Array.concat
        |> Array.map (fun c -> if c = 'X' then 1 else 0)
        |> Array.sum
    printfn $"{visitedCoordinates}"
    0
