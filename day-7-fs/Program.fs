open Simplee.APString

type Bag = Bag of string
type Rule = ContainBags of (int * Bag) list | NoBags

let parseContainList (list:string) =
    list.Split ", " |> Seq.map 
                           (fun item -> let [|number; desc; colour|] = item.Split ' ' |> Array.truncate 3
                                        (int number, Bag($"{desc} {colour}"))) |> Seq.toList   
let parseRule = function
        | StartsWith "no other bags" _ -> NoBags
        | containsList -> ContainBags(containsList |> parseContainList)             

let parseEntry (entry:string) = let [|bagName; ruledef|] = entry.Split " bags contain "; 
                                (Bag(bagName), ruledef |> parseRule)
                            
let rules = System.IO.File.ReadLines("input.txt") |> Seq.map parseEntry |> Map.ofSeq

let rec canHoldBag target holder =
   match rules.[holder] with
      | NoBags -> false 
      | ContainBags (bagList) -> bagList |> List.exists( fun (_, bag) -> bag = target || canHoldBag target bag) 

let rec getBagCount bag = 
    match rules.[bag] with
      | NoBags -> 0
      | ContainBags (bagList) -> bagList |> List.sumBy( fun (num, bag) -> num + (num * getBagCount bag))

rules |> Map.toSeq |> Seq.map fst |> Seq.filter (canHoldBag (Bag "shiny gold")) |> Seq.length |> printfn "Part 1: %i"
(getBagCount (Bag "shiny gold")) |> printfn "Part 2: %i"