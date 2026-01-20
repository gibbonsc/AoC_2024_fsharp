module Logic

let countTrue boolList =
    boolList |> List.filter id |> List.length

let getPairs (integers: int array) : (int * int) list =
    integers |> Array.toList |> List.pairwise

let isMonotone (integers: int array) : bool =
    let allIncreasing =
        getPairs integers
        |> List.forall (fun (a,b) -> (a < b))
    let allDecreasing =
        getPairs integers
        |> List.forall (fun (a,b) -> (a > b))
    allIncreasing || allDecreasing

let isGradual3 (integers: int array) : bool =
    getPairs integers
    |> List.forall (fun (a,b) -> (abs(b-a) < 4 && (a<>b)))

let arraysWithOneRemoved (integers: int array) : int array list =
    let n = Array.length integers
    if n=0 || n=1 then
        [ [| |]]
    else
        let mutable resultList : int array list = List.empty
        for s in 0 .. (n - 1) do
            let resultArray =
                if s=0 then
                    Array.sub integers 1 (n-1)
                elif s=(n-1) then
                    Array.sub integers 0 (n-1)
                else
                    Array.append (Array.sub integers 0 s) (Array.sub integers (s+1) (n-1-s))
            resultList <-
                List.append resultList [ resultArray ]
        resultList

let isDamp1Safe (integers: int array): bool =
    if isMonotone integers && isGradual3 integers then
        true
    else
        let dampenedArrays =
            arraysWithOneRemoved integers
        let dampenedArraysTests (arrays: int array list) : int =
            dampenedArrays
            |> List.map (fun a -> (isGradual3 a && isMonotone a))
            |> countTrue
        if dampenedArraysTests dampenedArrays > 0 then
            true
        else
            false
