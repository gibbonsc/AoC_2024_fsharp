module Main

// open Day3part1.Lib.Say
open Day3part1.Lib.Parse
open Expecto

[<Tests>]
let defaultTests =
    testList "Day3part1" [
        test "placeholder passes" {
            // hello "Mr. Schmedlap"
            Expect.isTrue true "This placeholder test should always pass"
        }
        test "empty input" {
            let result = scan ""
            let expected = []
            Expect.equal result expected "should return no tokens"
        }
        test "incomplete input" {
            let result = scan "A"
            let expected = []
            Expect.equal result expected "should return no tokens"
        }
        test "empty token arguments" {
            let result = scan "mul()"
            let expected = []
            Expect.equal result expected "mul() has no arguments"
        }
        test "one token" {
            let result = scan "mul(0,0)"
            let expected = ["mul(0,0)"]
            Expect.equal result expected "mul(0,0)"
        }
        test "token has one argument" {
            let result = scan "mul(0)"
            let expected = ["mul(0)"]
            Expect.equal result expected "mul(0) must be pruned later"
        }
        test "two tokens" {
            let result = scan "mul(12,345)mul(67,890)"
            let expected = ["mul(12,345)";"mul(67,890)"] |> List.rev
            Expect.equal result expected "mul(12,345)mul(67,890) two tokens"
        }
        test "two tokens between cruft" {
            let result = scan "tmul(10,11)uvwxmul(200,300)yz"
            let expected = ["mul(200,300)";"mul(10,11)"]
            Expect.equal result expected "two tokens between cruft"
        }
        test "three example AoC mul instructions" {
            let result = scan "xmul(2,4) mul(123,4) mul(44,46)"
            let expected = ["mul(44,46)";"mul(123,4)";"mul(2,4)"]
            Expect.equal result expected "three example AoC tokens"
        }
        test "example AoC corrupted section" {
            let result =
                scan """\
xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
"""
            let expected = ["mul(8,5)";"mul(11,8)";"mul(5,5)";"mul(2,4)"]
            Expect.equal result expected "example AoC corrupted section"
        }
    ]
