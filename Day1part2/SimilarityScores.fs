namespace Day1part2

module SimilarityScores =
    let counts ((candidates: int list), (queryData: int list)) : int list =
        // build a frequency map
        let freq =
            queryData
            // countBy obtains (value, frequency)
            |> List.countBy id
            // then produce an immutable balanced tree map:
            |> Map.ofList
        // look up the count of each candidate
        candidates
        |> List.map (
            fun (c : int) ->
                freq
                // Option<int> from the map lookup
                |> Map.tryFind c
                // convert None -> 0, Some n -> n
                |> Option.defaultValue 0
            )

    let scores ((candidates: int list), (queryData: int list)) : int list =
        // build a frequency map
        let freq =
            queryData
            // countBy obtains (value, frequency)
            |> List.countBy id
            // then produce an immutable balanced tree map:
            |> Map.ofList
        // look up the count of each candidate
        candidates
        |> List.map (
            fun (c : int) ->
                c * (  // multiply candidate by the discovered frequency
                    freq
                    // Option<int> from the map lookup
                    |> Map.tryFind c
                    // convert None -> 0, Some n -> n
                    |> Option.defaultValue 0
                )
            )