namespace Day6part2.Lib
open System.IO

// domain helper types
type Direction = Up | Right | Down | Left
type GuardState = {
    Dir: Direction
    Pos: (int * int)  // row, column position
}

// functions with intentional side effects
module Side =
    let getInput (path: string) : string array =
        File.ReadAllLines path

    // for debugging & entertaining animation
    let sidePrintAreaMap (grid: char array array) =
        System.Console.SetCursorPosition(0,0)
        let frameString =
            grid
            |> Array.map (fun r -> System.String(r))
            |> String.concat "\n"
        printf "\n%s" frameString

    // mutate grid contents with "breadcrumb" trail marks
    let sideTryMarkAreaMap
        (isDeputy: bool)
        (gSt: GuardState)
        (grid: char array array)
        : bool =
        let (row,col) = gSt.Pos
        // uppercase breadcrumbs for guard, lowercase for deputy
        let mark: char =
            if isDeputy then
                match gSt.Dir with
                | Up -> 'u'
                | Right -> 'r'
                | Down -> 'd'
                | Left -> 'l'
            else
                match gSt.Dir with
                | Up -> 'U'
                | Right -> 'R'
                | Down -> 'D'
                | Left -> 'L'
        if grid.[row].[col] = '.' || grid.[row].[col] = '^' then
            grid.[row].[col] <- mark
            true
        else
            // a breadcrumb was previously dropped
            false
