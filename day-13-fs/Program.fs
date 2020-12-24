let input = System.IO.File.ReadAllLines "input.txt"

let timestamp = input.[0] |> int
let busses = input.[1].Split "," |> Seq.filter ((<>) "x") |> Seq.map int |> Seq.toList

let timeTill bus timestamp = 
    let nextBus = (timestamp / bus) * bus + bus
    nextBus - timestamp

let bus, timeToWait = busses |> List.map (fun bus -> (bus, timestamp |> timeTill bus)) |> List.minBy (fun (_,waitTime) -> waitTime) 
printfn "Part 1 = %i" (bus * timeToWait)

let bussesIndexed = input.[1].Split "," |> Array.indexed |> Seq.filter (fun (_,t) -> t <> "x") 
                                        |> Seq.map (fun (pos, bus) -> (pos, int64 bus))
                                        |> Seq.toArray

let mutable _, value = bussesIndexed |> Array.head
let mutable increment = value;

for i, bus in bussesIndexed |> Array.skip 1 do
    let m = (-(int64)i % bus + bus) % bus;
    printfn "m = %i" m
    while (value % bus) <> m do
        value <- value + increment;
    increment <- increment * bus

printfn "Part 2: %i" value
  