module ParseTests

open Expecto
open Parse

let parseLineTests =
    testList "tryParseLine tests" [
        test "parses single integer" {
            let result = tryParseLine "42"
            Expect.equal result [| 42 |] "Should parse one number"
        }
        
        test "parses multiple space-separated integers" {
            let result = tryParseLine "7 6 4 2 1"
            Expect.equal result [| 7; 6; 4; 2; 1 |] "Should parse all numbers"
        }
        
        test "handles multiple spaces between numbers" {
            let result = tryParseLine "1    2     3"
            Expect.equal result [| 1; 2; 3 |] "Should handle extra spaces"
        }
        
        test "handles leading and trailing whitespace" {
            let result = tryParseLine "  5 10 15  "
            Expect.equal result [| 5; 10; 15 |] "Should trim spaces"
        }
        
        test "returns empty list for empty string" {
            let result = tryParseLine ""
            Expect.equal result [| |] "Empty string should give empty array"
        }
        
        // What should happen with invalid input?
        // test "handles line with non-integer" {
        //     let result = tryParseLine "1 abc 3"
        //     Expect.equal result [1; 3] "Should skip invalid values"
        //     // Or maybe: Expect.equal result [] "Should return empty on any error"
        //     // You decide the behavior!
        // }
    ]

let parseLinesTests =
    testList "tryParseLines tests" [
        test "parses multiple lines" {
            let input = [ "7 6 4 2 1"; "1 2 7 8 9"; "9 7 6 2 1"]
            let result = tryParseLines input
            let expected =
                [
                    [| 7; 6; 4; 2; 1 |];
                    [| 1; 2; 7; 8; 9 |];
                    [| 9; 7; 6; 2; 1 |]
                ]
            Expect.equal result expected "Should parse all lines"
        }
        
        test "handles empty sequence" {
            let result = tryParseLines []
            Expect.equal result [] "Empty input gives empty output"
        }
        
        test "handles mix of valid and invalid lines" {
            let input = ["1 2 3"; ""; "4 5 6"]
            let result = tryParseLines input
            // What should the middle empty line produce?
            let expected = [ [| 1; 2; 3 |]; [| |]; [| 4; 5; 6 |] ]
            Expect.equal result expected "Should handle empty lines"
        }
    ]

let allParseTests =
    testList "Parse module tests" [
        parseLineTests
        parseLinesTests
    ]