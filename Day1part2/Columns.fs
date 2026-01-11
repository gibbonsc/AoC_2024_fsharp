namespace Day1part2

module Columns =
    let toColumns (pairs: (int * int) array) : int list * int list =
        pairs
        |> Array.toList 
        |> List.unzip

    let toSortedColumns(pairs: (int * int) array) : int list * int list =
        let col1, col2 =
            pairs
            |> toColumns
        List.sort col1, List.sort col2
