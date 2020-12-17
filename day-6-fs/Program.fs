let groups = System.IO.File.ReadAllText("input.txt").Trim().Split "\n\n"
type Technique = AnyoneAnswered | EveryoneAnswered

let countQuestions technique (group:string) =    
    group.Split '\n' |> Seq.map Set.ofSeq
    |> match technique with | AnyoneAnswered -> Set.unionMany | EveryoneAnswered -> Set.intersectMany 
    |> Set.count

groups |> Seq.sumBy (countQuestions AnyoneAnswered)  |> printfn "Part 1: %i"
groups |> Seq.sumBy (countQuestions EveryoneAnswered) |> printfn "Part 2: %i"
