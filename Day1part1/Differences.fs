namespace Day1part1

module Differences =
    let differences (i1: int list, i2: int list) : int list =
        List.map2 (fun x y -> abs (x - y)) i1 i2

    let sumDiffs (i1: int list, i2: int list) : int =
        differences (i1, i2)
        |> List.sum
