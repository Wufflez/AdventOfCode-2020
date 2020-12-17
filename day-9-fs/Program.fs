let input = System.IO.File.ReadLines("input.txt") |> Seq.map int64 |> Seq.toArray

let valid (prevNums, number) =
    let uniquePrevNums = prevNums |> Set.ofSeq
    uniquePrevNums |> Set.exists (fun num -> uniquePrevNums.Remove(num) |> Set.contains(number - num))

let invalid item = not (valid item)

let iterateWithPrevious prevCount data = 
    data |> Seq.windowed (prevCount + 1) 
         |> Seq.map (fun wnd -> (wnd |> Seq.take prevCount, wnd |> Seq.last))

let findInvalid prevCount data = data |> iterateWithPrevious prevCount |> Seq.find invalid |> snd

let part1 = (findInvalid 25 input)
printfn "Part 1: First number = %i" part1

let findSumRange target (data: int64 array) = 
    let mutable h = 0
    let mutable t = 0
    let mutable total = data.[0]
    while total <> target do
        if total < target then
            h <- h + 1
            total <- total + data.[h]           
        else
            total <- total - data.[t]
            t <- t + 1 
    data.[t..h]   

let part2nums = findSumRange part1 input 
let min = part2nums |> Array.min
let max = part2nums |> Array.max
printfn "Part 2 nums: %A, max %i, min %i" part2nums max min
printfn "Part 2 nums sum: %i" (part2nums |> Array.sum)
printfn "Part 2: %i" (max + min)