let getSeatID (code:string) = code |> Seq.fold (fun n -> function |'B'|'R' -> 2*n+1 | _ -> 2*n) 0

let seatIDs = System.IO.File.ReadLines("input.txt") |> Seq.map getSeatID |> Seq.sort
printfn "Part 1: %i" (seatIDs |> Seq.max)

let mySeatId = seatIDs |> Seq.pairwise |> Seq.choose (function | a,b when b-a = 2 -> Some(b-1) | _ -> None) |> Seq.exactlyOne
printfn "Part 2: %i" mySeatId
