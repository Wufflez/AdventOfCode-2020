let testData = [1;2;4;8;20]

let rec findSumPair sum values = 
    let first = values |> List.head
    let rest = values |> list.head
    match rest |> List.tryFind(fun x -> x = sum - first)
        
    
let rec findSumTo sum n values =
    let first = values |> List.head
    let rest = values |> List.tail
    match n with
        | 2 ->  match rest |> List.tryFind((=) sum - first) with
                    | Some(f) -> Some [first; f]
                    | None -> None
        | _ ->  findSumTo (sum-first) (n-1) rest
       
 
//let findSumValues sum n values =
//    match n with        
//        | 2 -> for outer in values do
//                   for inner in List.tail values do  
//                        if outer + inner = sum then
//                            Some [outer; inner;]  
//                        else
//                            None

printfn "Sum values = %A" (findSumTo 24 2 testData)
    