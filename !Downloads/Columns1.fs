// Columns.fs
module Columns

/// Given a list of pairs (each line: a,b), return two lists: first column and second column.
let columns (pairs: (int * int) list) : int list * int list =
    // List.unzip is the idiomatic way to split tuples into two lists
    List.unzip pairs

/// Sorted columns using List.sort (ascending)
let sortedColumns (pairs: (int * int) list) : int list * int list =
    let c1, c2 = columns pairs
    List.sort c1, List.sort c2

/// If you prefer to name intermediates explicitly
let c1 (pairs: (int * int) list) : int list = pairs |> List.map fst
let c2 (pairs: (int * int) list) : int list = pairs |> List.map snd
let c1sorted (pairs: (int * int) list) : int list = c1 pairs |> List.sort
let c2sorted (pairs: (int * int) list) : int list = c2 pairs |> List.sort

/// Example: stable sort with custom key (same as List.sort for ints, but shown for completeness)
let c1sortedByNeg (pairs: (int * int) list) : int list = c1 pairs |> List.sortBy (fun x -> -x)
