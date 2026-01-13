module Logic

let countTrue boolList =
    boolList |> List.filter id |> List.length

// let processData input =
//     // Your processing logic here
//     input |> List.map (fun x -> x > 5)

let getPairs (integers: int list) : (int * int) list =
    integers |> List.pairwise

let isMonotone (integers: int list) : bool =
    let allIncreasing =
        getPairs integers
        |> List.forall (fun (a,b) -> (a < b))
    let allDecreasing =
        getPairs integers
        |> List.forall (fun (a,b) -> (a > b))
    allIncreasing || allDecreasing

let isGradual3 (integers: int list) : bool =
    getPairs integers
    |> List.forall (fun (a,b) -> (abs(b-a) < 4 && (a<>b)))
