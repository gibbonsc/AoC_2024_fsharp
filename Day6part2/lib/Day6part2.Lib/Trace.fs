namespace Day6part2.Lib

module Trace =
    let mapToStringList (grid: char array array): string list =
        grid
        |> Array.map (fun r -> System.String(r))
        |> Array.toList
    let copyMap (grid: char array array): char array array =
        grid
        |> mapToStringList
        |> List.toArray
        |> Parse.getAreaMap

    let turn (guard: GuardState) : GuardState =
        match guard.Dir with
        | Up -> { guard with Dir = Right}
        | Right -> { guard with Dir = Down }
        | Down -> { guard with Dir = Left }
        | Left -> { guard with Dir = Up }

    let oneStep (guard: GuardState): (int * int) =
        let (row, col) = guard.Pos
        match guard.Dir with
        | Up ->
            (row - 1), col
        | Right ->
            row, (col + 1)
        | Down ->
            (row + 1), col
        | Left ->
            row, (col - 1)

    let lookAhead (guard: GuardState) (grid: char array array): char =
        let (ahead: int * int) = oneStep guard
        let (row, col) = ahead
        if row < 0 || row >= (Array.length grid) ||
            col < 0 || col >= (Array.length grid.[0]) then
            '!'  // use bang to represent "outside grid boundary"
        else
            grid.[row].[col]

    let moveStep (guard: GuardState) (grid: char array array): GuardState =
        let observeAhead: char = lookAhead guard grid
        if observeAhead = '!' then
            guard
        elif observeAhead = '#' then
            turn guard  // just turn, don't move quite yet
        else
            { guard with Pos = oneStep guard }
        (*
            The following code missed some potential obstacles
            by moving too quickly after encountering an obstacle
        *)
        // elif observeAhead <> '#' then
        //     { guard with Pos = oneStep guard }
        // else
        //     let turnedGuard = turn guard
        //     let observeAside: char = lookAhead turnedGuard grid
        //     if observeAside = '!' then
        //         turnedGuard
        //     elif observeAside <> '#' then
        //         { turnedGuard with Pos = oneStep turnedGuard }
        //     else
        //         let reversedGuard = turn turnedGuard
        //         if lookAhead reversedGuard grid = '!' then
        //             reversedGuard
        //         else
        //             { reversedGuard with Pos = oneStep reversedGuard }

    let checkRetrace (guard: GuardState) (crumb: char): bool =
        // true if loop detected (that is, confirmed move retraced)
        match crumb with
        | 'U' | 'u' -> if guard.Dir = Up then true else false
        | 'R' | 'r' -> if guard.Dir = Right then true else false
        | 'D' | 'd' -> if guard.Dir = Down then true else false
        | 'L' | 'l' -> if guard.Dir = Left then true else false
        | _ -> false

    let checkExtraObstacle
        (guard: GuardState)
        (grid: char array array)
        : bool =
        // could a new untried obstacle be placed ahead?
        let occupant = lookAhead guard grid
        if occupant <> '.' then false
        else
            // prepare to dispatch a deputy guard
            let gridCopy = copyMap grid
            // place temporary obstacle into state
            let aheadRow, aheadCol = oneStep guard
            gridCopy.[aheadRow].[aheadCol] <- '#'
            // summon deputy
            let mutable deputy: GuardState = turn guard
            let mutable deputyMark: bool = true
            let mutable footMark: char = gridCopy.[fst deputy.Pos].[snd deputy.Pos]
            let mutable observed: char = lookAhead deputy gridCopy
            // make deputy trace until escape or loop
            while not ('!' = observed || checkRetrace deputy footMark) do
                deputy <- moveStep deputy gridCopy
                footMark <- gridCopy.[fst deputy.Pos].[snd deputy.Pos]
                deputyMark <- Side.sideTryMarkAreaMap true deputy gridCopy
                observed <- lookAhead deputy gridCopy
                // Side.sidePrintAreaMap gridCopy
                // printf $" {deputy.Dir} {deputy.Pos} {footMark} {observed} "
                // System.Console.ReadKey() |> ignore
            if '!' = observed then
                // Side.sidePrintAreaMap gridCopy
                // printf "Deputy escaped                                    "
                // System.Console.ReadKey() |> ignore
                false
            else
                // Side.sidePrintAreaMap gridCopy
                // printf $"obstacle at {aheadRow},{aheadCol} loops deputy"
                // System.Console.ReadKey() |> ignore
                true
