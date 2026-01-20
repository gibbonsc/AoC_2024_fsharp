module Program

open Expecto
let allTests =
    testList "All tests" [
        ParseTests.allParseTests
        LogicTests.allLogicTests
    ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv allTests
