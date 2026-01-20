module LogicTests

open Expecto
open Logic

let booleanTests =
    testList "countTrue" [
        test "two among six" {
            let results = [true; false; false; false; false; true]
            Expect.equal (countTrue results) 2 "Should count 2 true values"
        }

        test "ten among ten" {
            let results = [true; true; true; true; true;
                true; true; true; true; true]
            Expect.equal (countTrue results) 10 "Should count 10 true values"
        }
        
        test "empty" {
            Expect.equal (countTrue []) 0 "Empty list should be 0"
        }
    ]

let isMonotoneTests =
    testList "isMonotone" [
        test "strictly increasing" {
            let values = [| 2; 3; 5; 6; 7; 8 |]
            Expect.equal (isMonotone values) true "assert true"
        }
        test "strictly decreasing" {
            let values = [| 99; 96; 93; 90 |]
            Expect.equal (isMonotone values) true "assert true"
        }
        test "nondecreasing" {
            let values = [| 2; 3; 4; 4; 5 |]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "nonincreasing" {
            let values = [| 87; 84; 81; 78; 75; 72; 69; 69 |]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "increasing then decreasing" {
            let values = [| -1; 0; 1; 2; 3; 4; -1 |]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "decreasing then increasing" {
            let values = [| -1; -2; -3; -4; -3; -2; 0; 2; 42 |]
            Expect.equal (isMonotone values) false "assert false"
        }
    ]

let isGradual3Tests = 
    testList "isGradal3" [
        test "adjustments by 1 or 2 among 6" {
            let values = [| 2; 3; 5; 6; 7; 8 |]
            Expect.equal (isGradual3 values) true "assert true"
        }
        test "adjustments by 1 to 3 among 5" {
            let values = [| 9; 6; 4; 2; 1 |]
            Expect.equal (isGradual3 values) true "assert true"
        }
        test "an adjustment by +4" {
            let values = [| 1; 2; 6; 8; 9 |]
            Expect.equal (isGradual3 values) false "assert false"
        }
        test "an adjustment by -4" {
            let values = [| 1; 0; -1; -3; -7 |]
            Expect.equal (isGradual3 values) false "assert false"
        }
        test "a steady nonadjustment" {
            let values = [| 8; 6; 4; 4; 1 |]
            Expect.equal (isGradual3 values) false "assert false"
        }
    ]

let arraysWithOneRemovedTests =
    testList "arraysWithOneRemoved" [
        test "edge case: empty argument" {
            let value = [| |]
            Expect.equal (arraysWithOneRemoved value) [
                [| |]
            ] "empty array"
        }
        test "edge case: singleton" {
            let value = [| 0 |]
            Expect.equal (arraysWithOneRemoved value) [
                [| |]
            ] "empty array"
        }
        test "array of size 2" {
            let value = [| -2; -1 |]
            Expect.equal (arraysWithOneRemoved value) [
                [| -1 |]; [| -2 |]
            ] "two singleton arrays"
        }
        test "array of size 4" {
            let value = [| 81; 82; 83; 84 |]
            Expect.equal (arraysWithOneRemoved value) [
                [| 82; 83; 84 |]; [| 81; 83; 84 |];
                [| 81; 82; 84 |]; [| 81; 82; 83 |]
            ] "four arrays of size 3"
        }
        test "large array size 895" {
            let value = [| for x in 5 .. 899 -> x |]
            let n = value.Length
            let expected =
                [ for i in 0 .. n - 1 ->
                    [| for j in 0 .. n - 1 do
                        if j <> i then yield value.[j] |] ]
            let actual = arraysWithOneRemoved value
            Expect.equal actual expected "remove each position exactly once"
        }
        // equivalent to:
        // testCase "large: 0 < x < 900" <| fun _ ->
        //     let value = [| for x in 1 .. 899 -> x |]
        //     let n = value.Length
        //     let expected =
        //         [ for i in 0 .. n - 1 ->
        //             [| for j in 0 .. n - 1 do
        //                 if j <> i then yield value.[j] |] ]
        //     let actual = arraysWithOneRemoved value
        //     Expect.equal actual expected "remove each position exactly once"
    ]

let pipelineTests =
    testList "pipeline operation" [
        test "test and count" {
            let values = [ [| 7; 6; 4; 2; 1 |]; [| 1; 2; 7; 8; 9 |]; [| 8; 6; 4; 4; 1 |] ]
            let count = values |> List.map (fun a -> (isMonotone a) && (isGradual3 a)) |> countTrue
            Expect.equal count 1 "Should have 1 test that passes"
        }
    ]

let isDamp1SafeTests =
    testList "isDamp1Safe" [
        test "gradual decreasing" {
            let values =
                [| 7; 6; 4; 2; 1 |]
            Expect.equal (isDamp1Safe values) true "assert true - no removals necessary"
        }
        test "steep increasing" {
            let values =
                [| 1; 2; 7; 8; 9 |]
            Expect.equal (isDamp1Safe values) false "assert false - no removals dampen"
        }
        test "steep decreasing" {
            let values =
                [| 9; 7; 6; 2; 1 |]
            Expect.equal (isDamp1Safe values) false "assert false - no removals dampen"
        }
        test "decreased only once" {
            let values =
                [| 1; 3; 2; 4; 5 |]
            Expect.equal (isDamp1Safe values) true "assert true - removing 3 dampens"
        }
        test "steady once" {
            let values =
                [| 8; 6; 4; 4; 1 |]
            Expect.equal (isDamp1Safe values) true "assert true - removing 4 dampens"
        }
        test "gradual increasing" {
            let values =
                [| 1; 3; 6; 7; 9 |]
            Expect.equal (isDamp1Safe values) true "assert true - no removals necessary"
        }
    ]

let allLogicTests =
    testList "Logic module tests" [
        booleanTests
        isMonotoneTests
        isGradual3Tests
        arraysWithOneRemovedTests
        pipelineTests
        isDamp1SafeTests
    ]
