//let input = System.IO.File.ReadAllLines "input.txt"
let input = [|"939";"7,13,x,x,59,x,31,19"|]

let timestamp = input.[0] |> int
let busses = input.[1].Split "," |> Seq.filter ((<>) "x") |> Seq.map int |> Seq.toList

let timeTill bus timestamp = 
    let nextBus = (timestamp / bus) * bus + bus
    nextBus - timestamp

let bus, timeToWait = busses |> List.map (fun bus -> (bus, timestamp |> timeTill bus)) |> List.minBy (fun (_,waitTime) -> waitTime) 
printfn "Part 1 = %i" (bus * timeToWait)

let bussesIndexed = input.[1].Split "," |> Array.indexed |> Seq.filter (fun (_,t) -> t <> "x") 
                                        |> Seq.map (fun (pos, bus) -> (pos, int64 bus))
                                        |> Seq.sortByDescending (fun (_, bus)->bus) |> Seq.toArray

let (bigPos, biggestBus) = bussesIndexed |> Array.head

let biggestBusTimestamp = Seq.initInfinite (fun n -> (int64)n * biggestBus) 
                            |> Seq.find (fun ts -> bussesIndexed |> Seq.forall( fun (pos, bus) -> (ts - (int64)bigPos + (int64)pos) % (int64)bus = 0L))

printfn "Part 2 = %i" (biggestBusTimestamp - (int64)bigPos)

// TODO:  Do this using that Chinese remainder theorum apparently is the way you're supposed to do this.