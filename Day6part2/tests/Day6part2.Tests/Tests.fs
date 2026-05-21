module Day6part2.Tests

open Expecto
open Day6part2.Lib

[<Tests>]
let tester =
    testList "Tester test" [
        testCase "arith" <| fun _ ->
            Expect.equal (1 + 2) 3 "Arithmetic Tester"
    ]

[<Tests>]
let inputTests =
    testList "Input tests" [
        testCase "example.txt" <| fun _ ->
            Expect.equal
                ((Side.getInput "example.txt").Length)
                10
                "Read 10 lines from example.txt"

        fun _ ->
            let input = Side.getInput "example19x13.txt"
            let got = Array.length input
            let want = 13
            Expect.equal got want "Read 13 lines from example19x13.txt"
        |> testCase "example19x13.txt"

        testCase "input.txt" <| fun _ ->
            let want = 130
            let got = Array.length (Side.getInput "input.txt")
            Expect.equal got want "Read 130 lines from input.txt"
    ]

[<Tests>]
let parseCoordTests =
    let parseCases =
        [
            "example.txt", (10,10), (6,4)
            "example19x13.txt", (13,19), (12,0)
            "input.txt", (130,130), (44,69)
        ]
    testList "Parse" [
        testList "mapDimensions" [
            for fileName, want, _ in parseCases do
                fun _ ->
                    let got =
                        Side.getInput fileName
                        |> Parse.mapDimensions
                    Expect.equal got want "dimensions should match"
                |> testCase $"{fileName} dimensions {want}"
        ]
        testList "guardStartPos" [
            for fileName, _, want in parseCases do
                fun _ ->
                    let got =
                        Side.getInput fileName
                        |> Parse.guardStartPos
                    Expect.equal got want "initial guard position should match"
                |> testCase $"{fileName} guard at {want}"
        ]
    ]

[<Tests>]
let parseContentTests =
    let getMap fileName =
        fileName
        |> Side.getInput
        |> Parse.getAreaMap
    testList "Parse content" [
        testCase "example.txt char count" <| fun _ ->
            let got = getMap "example.txt" |> Array.concat
            Expect.equal got.Length 100 "char count"

        testCase "example.txt content" <| fun _ ->
            let got = getMap "example.txt"
            let want = [|
                    "....#....." |> Seq.toArray
                    ".........#" |> Seq.toArray
                    ".........." |> Seq.toArray
                    "..#......." |> Seq.toArray
                    ".......#.." |> Seq.toArray
                    ".........." |> Seq.toArray
                    ".#..^....." |> Seq.toArray
                    "........#." |> Seq.toArray
                    "#........." |> Seq.toArray
                    "......#..." |> Seq.toArray
            |]
            Expect.equal got want "10-by-10 jagged array of arrays of chars from example.txt"

        fun _ ->
            let areaMap =
                Side.getInput "input.txt"
                |> Parse.getAreaMap
            let charSamples = [
                areaMap.[0].[0]; areaMap.[1].[125]; areaMap.[2].[65];
                areaMap.[44].[69]; areaMap.[44].[71]; areaMap.[85].[24];
                areaMap.[127].[100]; areaMap.[128].[92]; areaMap.[129].[129]
            ]
            Expect.equal
                charSamples
                ['.';'#';'.';'^';'.';'#';'.';'#';'.']
                "correct input.txt sample char values"
        |> testCase "correct input.txt samples"

        testCase "guardStartPos without initial location" <| fun _ ->
            let grid = [| "..."; "#.."; "..."; "..#"|]
            Expect.throws
                (fun () -> Parse.guardStartPos grid |> ignore)
                "should fail when '^' is missing"

        testCase "message from missing guardStartPos" <| fun _ ->
            let grid = [| "..."; "#.."; "..."; "..#" |]
            try
                Parse.guardStartPos grid |> ignore
                failtest "Expected Parse.guardStartPos to throw"
            with
            | Failure msg ->
                Expect.equal msg "Couldn't find '^' in input" "exception message should match"
            | ex ->
                failtestf "Unexpected exception type: %s" (ex.GetType().FullName)
    ]

[<Tests>]
let traceTests =
    testList "Trace" [
        testCase "turn to Right" <| fun _ ->
            let got = Trace.turn { Dir=Up; Pos=0,0 }
            Expect.equal got { Dir=Right; Pos=0,0 } "turn Up>Right"
        testCase "turn to Down" <| fun _ ->
            let got = Trace.turn { Dir=Right; Pos=0,0 }
            Expect.equal got { Dir=Down; Pos=0,0 } "turn Right>Down"
        testCase "turn to Left" <| fun _ ->
            let got = Trace.turn { Dir=Down; Pos=0,0 }
            Expect.equal got { Dir=Left; Pos=0,0 } "turn Down>Left"
        testCase "turn to Up" <| fun _ ->
            let got = Trace.turn { Dir=Left; Pos=0,0 }
            Expect.equal got { Dir=Up; Pos=0,0 } "turn Left>Up"
        testList "oneStep" [
            let stepCases =
                [
                    (1,1), Up, (0,1), "decrement row to 0"
                    (1,1), Right, (1,2), "increment col to 2"
                    (1,1), Down, (2,1), "increment row to 2"
                    (1,1), Left, (1,0), "decrement col to 0"
                    (98,76), Up, (97,76), "decrement row to 97"
                    (54,32), Right, (54,33), "increment col to 33"
                    (99,88), Down, (100,88), "increment row to 100"
                    (129,100), Left, (129,99), "decrement col to 99"
                ]
            for initialPos, dir, wantPos, desc in stepCases do
                testCase desc <| fun _ ->
                    let gs: GuardState = { Dir = dir; Pos = initialPos }
                    let gotPos = Trace.oneStep gs
                    Expect.equal gotPos wantPos desc
        ]
        testList "lookAhead" [
            let testMap =
                [|
                    [| '.'; '#'; '.' |]
                    [| '.'; '.'; '#' |]
                    [| '.'; 'X'; '.' |]
                |]
            let lookCases =
                [
                    (1,1), Up, '#', "look up"
                    (1,1), Right, '#', "look right"
                    (1,1), Down, 'X', "look down"
                    (1,1), Left, '.', "look left"
                    (0,0), Up, '!', "top edge"
                    (2,2), Right, '!', "right edge"
                    (2,2), Down, '!', "bottom edge"
                    (0,0), Left, '!', "left edge"
                ]
            for initialPos, dir, wantChar, desc in lookCases do
                testCase desc <| fun _ ->
                    let gs: GuardState = { Dir = dir; Pos = initialPos }
                    let gotChar = Trace.lookAhead gs testMap
                    Expect.equal gotChar wantChar desc
        ]
        testList "moveStep" [
            let (testGrid1: string list) = [
                ".#...."
                ".XXXX#"
                "#X..#."
                "......"
            ]
            let (testMap1: char array array) =
                List.map Seq.toArray testGrid1
                |> List.toArray
            let moveStepCases1 = [
                // changed moveStep: do NOT advance after turn
                { Dir = Up; Pos = 2,1 },    { Dir = Up; Pos = 1,1 },    "advance up"
            //  { Dir = Up; Pos = 1,1 },    { Dir = Right; Pos = 1,2 }, "turn, advance right"
                { Dir = Up; Pos = 1,1 },    { Dir = Right; Pos = 1,1 }, "turn right"
                { Dir = Right; Pos = 1,3 }, { Dir = Right; Pos = 1,4 }, "advance right"
            //  { Dir = Right; Pos = 1,4 }, { Dir = Left; Pos = 1,3 },  "flip, advance left"
                { Dir = Right; Pos = 1,4 }, { Dir = Down; Pos = 1,4 },  "turn down"
            //  { Dir = Down; Pos = 1,4 },  { Dir = Left; Pos = 1,3 },  "turn, advance left"
                { Dir = Down; Pos = 1,4 },  { Dir = Left; Pos = 1,4 },  "turn left"
                { Dir = Left; Pos = 2,2 },  { Dir = Left; Pos = 2,1 },  "advance left"
            //  { Dir = Left; Pos = 2,1 },  { Dir = Up; Pos = 1,1 },    "turn, advance up"
                { Dir = Left; Pos = 2,1 },  { Dir = Up; Pos = 2,1 },    "turn up"
                { Dir = Up; Pos = 0,3 },    { Dir = Up; Pos = 0,3 },    "reached edge"
                { Dir = Up; Pos = 2,5 },    { Dir = Right; Pos = 2,5 }, "turn, facing edge"
            //  { Dir = Left; Pos = 2,5},   { Dir = Right; Pos = 2,5 }, "flip, facing edge"
                { Dir = Left; Pos = 2,5},   { Dir = Up; Pos = 2,5 },    "turn up near edge"
            ]
            for initGuardState, wantGuardState, desc in moveStepCases1 do
                testCase desc <| fun _ ->
                    Expect.equal
                        (Trace.moveStep initGuardState testMap1)
                        wantGuardState
                        desc
        ]

        testList "checkExtraObstacle" [
            let checkCases1 = [
                [
                    ".#...."
                    ".XX..#"
                    "#X...."
                    "...#.."
                ], { Dir=Right; Pos=1,2 }, false, "down escapes"
                [
                    ".#...."
                    ".XXX.#"
                    "#X...."
                    "...#.."
                ], { Dir=Right; Pos=1,3 }, true, "down then left finds"
                [
                    ".#...."
                    ".XXXX#"
                    "#X...."
                    "...#.."
                ], { Dir=Down; Pos=1,4 }, false, "left escapes"
                [
                    ".#...."
                    ".XXXX#"
                    "#X..X."
                    "...#.."
                ], { Dir=Down; Pos=2,4 }, true, "left finds"
            ]
            // insufficient coverage, but finished testing manually anyway...
            // found much better "checkExtraObstacle" test case at:
            // https://www.reddit.com/r/adventofcode/comments/1h7tovg/comment/m0stkxk/

            for gridSt, gSt, want, desc in checkCases1 do
                let testMap1: char array array =
                    List.map Seq.toArray gridSt
                    |> List.toArray
                testCase $"checkExtraObstacle {desc}" <| fun _ ->
                    let got = Trace.checkExtraObstacle gSt testMap1
                    Expect.equal got want desc
        ]
    ]
