namespace Day3part1.Lib

module Parse =
    (* evaluate a mul(n,k) token (integer n, k) *)
    let evalMul (mulInstruction : string) : int =
        let stringLength = mulInstruction.Length
        let intArgs = mulInstruction.[4..(stringLength - 2)]
        let ints = intArgs.Split [| ',' |]
        if ints.Length <> 2 then
            // return 0 if more or fewer than 2 arguments
            0
        else
            (int ints[0]) * (int ints[1])

    (*
    scan for mul(,) tokens,
      with decimal digits before and after the comma, such as
      mul(0,0) or mul(999,999)
    currently finds single-argument tokens like mul(0) or mul(12);
      those will need pruning later
    also finds three+-argument tokens mul(0,0,0) or mul(0,0,0,0),
      and tokens with four-digit arguments mul(1234,5678);
      fortunately my input didn't feature any of those, but if it did,
      those would also need pruning later
    *)
    let scan (arg : string) : string list =
        let rec scanR (chars : char list) (bench : string) (tokens : string list) : string list =
            match chars with
            | head :: tail ->
                match head with
                | 'm' ->
                    scanR tail "m" tokens
                | 'u' ->
                    if bench = "m" then
                        scanR tail "mu" tokens
                    else
                        scanR tail "" tokens
                | 'l' ->
                    if bench = "mu" then
                        scanR tail "mul" tokens
                    else
                        scanR tail "" tokens
                | '(' ->
                    if bench = "mul" then
                        scanR tail "mul(" tokens
                    else
                        scanR tail "" tokens
                | ',' ->
                    if bench.Length >= 5 &&
                        bench.[0..3] = "mul(" then                    
                        scanR tail (bench + ",") tokens
                    else
                        scanR tail "" tokens
                | ')' ->
                    if bench.Length >= 5 &&
                        bench.[0..3] = "mul(" then
                        scanR tail "" ((bench + ")") :: tokens)
                    else
                        scanR tail "" tokens
                | c when '0' <= c && c <= '9' ->
                    if bench.Length >= 4 &&
                        bench.[0..3] = "mul(" then
                        scanR tail (bench + string c) tokens
                    else
                        scanR tail "" tokens
                | _ -> scanR tail "" tokens
            | [] -> tokens
        scanR (arg |> Seq.toArray |> Array.toList) "" []
