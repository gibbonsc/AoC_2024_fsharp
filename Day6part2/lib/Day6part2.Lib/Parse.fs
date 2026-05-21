namespace Day6part2.Lib

module Parse =
    let mapDimensions (mapGrid: string array): (int * int) =
        let rows = Array.length mapGrid
        let cols =
            match rows with
            | 0 -> 0
            | _ -> Seq.length mapGrid.[0]
        rows, cols
    let getAreaMap (mapGrid: string array): char array array =
        mapGrid
        |> Array.map (fun x -> Seq.toArray x)
    let guardStartPos (mapGrid: string array): (int * int) =
        let (initialRow: int option) =
            Array.tryFindIndex (fun (x: string) -> x.Contains('^')) mapGrid
        if initialRow = None then
            failwith "Couldn't find '^' in input"
        let (initialCol: int) =
            mapGrid.[initialRow.Value].IndexOf('^')
        (initialRow.Value,initialCol)
