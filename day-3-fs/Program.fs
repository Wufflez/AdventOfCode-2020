let countSlopeTrees (data:string array) (across, down)  =    
    let width = data.[0].Length
    seq { 0..down..data.Length-1 } |> Seq.filter (fun y -> data.[y].[((y/down) * across) % width] = '#') |> Seq.length |> int64
   
let data = System.IO.File.ReadAllLines "input.txt"    
let partOne = countSlopeTrees data (3,1)
printfn "Part 1: %i" partOne

let partTwo = [1,1; 3,1; 5,1; 7,1; 1,2] |> Seq.map (countSlopeTrees data) |> Seq.fold (*) 1L
printfn "Part 2: %i" partTwo
