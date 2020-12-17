open FSharp.Text.RegexExtensions
open FSharp.Text.RegexProvider


type PasswordRegex = Regex< @"(?<a>\d+)+-(?<b>\d+)\s(?<ch>\w):\s(?<pw>.*)" >
type ValidationType = Occurance | Position 

let (|PasswordEntry|) str = let entry = PasswordRegex().TypedMatch(str)                            
                            (entry.a.AsInt, entry.b.AsInt, entry.ch.AsChar, entry.pw.Value)

let validateEntry validation = function
    | PasswordEntry (a,b,ch,pw) -> match validation with
                                   | Occurance -> let count = pw |> Seq.filter((=) ch) |> Seq.length
                                                  count >= a && count <= b
                                   | Position -> (pw.[a-1] = ch) <> (pw.[b-1] = ch)

let inputData = System.IO.File.ReadLines "input.txt"
printfn "Part 1: %i" (inputData |> Seq.filter (validateEntry Occurance) |> Seq.length)
printfn "Part 2: %i" (inputData |> Seq.filter (validateEntry Position) |> Seq.length)