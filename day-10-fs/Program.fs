let Sequence available =
    let rec SequenceInner adapters joltage available =       
        let potentialnext = available |> Set.filter ((>=)(joltage + 3L)) |> Seq.sort |> Seq.toList
        match potentialnext |> List.tryHead with
            | Some adapter -> SequenceInner (adapter::adapters) adapter (available |> Set.remove adapter) 
            | None -> adapters
    List.rev (SequenceInner [] 0L available)

let adapters = System.IO.File.ReadLines "input.txt" |> Seq.map int64 |> Set.ofSeq
let differences = 0L::(Sequence adapters) |> Seq.pairwise |> Seq.map(fun (a,b)->b-a) |> Seq.countBy id |> Map.ofSeq

printfn "Part 1: %i" ((differences.Item 1L) * ((differences.Item 3L)+1)) // Extra +3 here for the step to device

let RouteCount available =
    let mutable lookup = Map.empty
    let rec RouteCountInner joltage available =          
        let potentialnext = available |> Set.filter (fun adp -> adp <= joltage+3L && adp >= joltage) |> Seq.sort |> Seq.toArray        
        match (lookup.TryFind joltage) with
            | Some routeCount -> routeCount
            | None -> let routeCount =
                          match potentialnext with
                              | [||] -> 1L
                              | _ -> potentialnext |> Seq.sumBy (fun adp -> RouteCountInner adp (available |> Set.remove adp))
                      lookup <- Map.add joltage routeCount lookup
                      routeCount
    RouteCountInner 0L available

printfn "Route count: %i" (RouteCount adapters)