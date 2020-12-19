let rec findSumTo sum n values =   
    match values with
        | first :: rest -> 
            match n with 
                | 2 -> match rest |> List.tryFind ((=) (sum - first)) with
                        | Some other -> [first; other]
                        | None -> findSumTo sum 2 rest
                | _ -> match findSumTo (sum-first) (n-1) rest with
                        | [] -> findSumTo sum n rest
                        | others -> (first::others)                        
        | _ -> []

let input = System.IO.File.ReadAllLines "input.txt" |> Seq.map int |> Seq.toList

findSumTo 2020 2 input |> List.fold (*) 1 |> printfn "Part 1: %i"
findSumTo 2020 3 input |> List.fold (*) 1 |> printfn "Part 2: %i"  