open Day6part2.Lib

[<EntryPoint>]
let main args =
    let inputPath =
        match args.Length with
        | 0 -> "example.txt"
        | _ -> args.[0]
    let inputLines = Side.getInput inputPath
    // inputLines |> Array.iter (printfn "%s")

    let (mapRowCount,mapColCount) =
        Parse.mapDimensions inputLines
    let areaMap =
        Parse.getAreaMap inputLines
    let (startGuardRow,startGuardCol) =
        Parse.guardStartPos inputLines

    // System.Console.Clear()
    // Side.sidePrintAreaMap areaMap
    // printfn $"\n{mapRowCount} rows {mapColCount} cols"
    // printfn $"guard at {startGuardRow},{startGuardCol}"
    let mutable guard: GuardState =
        {
            Pos = startGuardRow,startGuardCol
            Dir = Up
        }
    let mutable loopsDetected: int = 0
    let mutable loopObstacleLocations: (int * int) list = []
    
    Side.sideTryMarkAreaMap false guard areaMap |> ignore
    while '!' <> Trace.lookAhead guard areaMap do
        Side.sideTryMarkAreaMap false guard areaMap |> ignore
        // Side.sidePrintAreaMap areaMap
        // printf " : : : : : : : : : : : : : : : : : : : : : : : : : "
        // System.Console.ReadKey() |> ignore
        if Trace.checkExtraObstacle guard areaMap then
            loopObstacleLocations <- (Trace.oneStep guard) :: loopObstacleLocations
            loopsDetected <- loopsDetected + 1
        guard <- Trace.moveStep guard areaMap
    Side.sideTryMarkAreaMap false guard areaMap |> ignore
    // Side.sidePrintAreaMap areaMap
    // printfn " : : : : : : : : : : : : : : : : : : : : : : : : : "
    // System.Console.ReadKey() |> ignore

    (*count visited areaMap cells*)
    let (visitedCoordinates: int) =
        let crumbs: char list = [ 'U'; 'u'; 'R'; 'r'; 'D'; 'd'; 'L'; 'l' ]
        areaMap
        |> Array.concat
        |> Array.map (fun c -> if (crumbs |> List.contains c) then 1 else 0)
        |> Array.sum
    printfn $"{visitedCoordinates} {loopsDetected}"
    // List.iter (fun p -> printf $" {p}") loopObstacleLocations

    // let origAreaMap =
    //     Parse.getAreaMap inputLines

    0
