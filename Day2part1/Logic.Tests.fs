module LogicTests

open Expecto
open Logic

let booleanTests =
    testList "Boolean counting" [
        test "counts true among six values" {
            let results = [true; false; false; false; false; true]
            Expect.equal (countTrue results) 2 "Should count 2 true values"
        }

        test "counts true among ten values" {
            let results = [true; true; true; true; true;
                true; true; true; true; true]
            Expect.equal (countTrue results) 10 "Should count 10 true values"
        }
        
        test "empty list returns zero" {
            Expect.equal (countTrue []) 0 "Empty list should be 0"
        }
    ]

let isMonotoneTests =
    testList "isMonotone testing" [
        test "strictly increasing" {
            let values = [2; 3; 5; 6; 7; 8]
            Expect.equal (isMonotone values) true "assert true"
        }
        test "strictly decreasing" {
            let values = [99; 96; 93; 90]
            Expect.equal (isMonotone values) true "assert true"
        }
        test "strictly nondecreasing" {
            let values = [2; 3; 4; 4; 5]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "strictly nonincreasing" {
            let values = [87; 84; 81; 78; 75; 72; 69; 69]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "increasing then decreasing" {
            let values = [-1; 0; 1; 2; 3; 4; -1]
            Expect.equal (isMonotone values) false "assert false"
        }
        test "decreasing then increasing" {
            let values = [-1; -2; -3; -4; -3; -2; 0; 2; 42]
            Expect.equal (isMonotone values) false "assert false"
        }
    ]

let isGradual3Tests = 
    testList "isGradual3 testing" [
        test "adjustments by 1 or 2 among 6" {
            let values = [2; 3; 5; 6; 7; 8]
            Expect.equal (isGradual3 values) true "assert true"
        }
        test "adjustments by 1 to 3 among 5" {
            let values = [9; 6; 4; 2; 1]
            Expect.equal (isGradual3 values) true "assert true"
        }
        test "adjustments one of which is by +4" {
            let values = [1; 2; 6; 8; 9]
            Expect.equal (isGradual3 values) false "assert false"
        }
        test "adjustments one of which is by -4" {
            let values = [1; 0; -1; -3; -7]
            Expect.equal (isGradual3 values) false "assert false"
        }
        test "adjustments one of which is steady" {
            let values = [8; 6; 4; 4; 1]
            Expect.equal (isGradual3 values) false "assert false"
        }
    ]

// let pipelineTests =
//     testList "Pipeline operations" [
//         test "processes and counts" {
//             let input = [1; 6; 3; 8; 2; 9]
//             let count = input |> processData |> countTrue
//             Expect.equal count 3 "Should have 3 values > 5"
//         }
//     ]

let allLogicTests =
    testList "All tests" [
        booleanTests
        isMonotoneTests
        isGradual3Tests
        // pipelineTests
    ]