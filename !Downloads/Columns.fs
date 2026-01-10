// Columns.fs
module Columns

/// Split a list of (int * int) into two lists
let columns (pairs: (int * int) list) : int list * int list =
    List.unzip pairs

/// Sorted columns (ascending)
let sortedColumns (pairs: (int * int) list) : int list * int list =
    let c1, c2 = columns pairs
    List.sort c1, List.sort c2
