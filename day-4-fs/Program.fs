open System.IO
open System
open System.Text.RegularExpressions

type Field = { Name: string; Value: string} 
type Passport = Field list
type Height = | Centimeters of int | Inches of int

let parseField (field:string) = match field.Split ':' with | [|a;b|] -> {Name=a; Value=b} 

let parsePassport (data:string) = data.Split ([|'\n';' '|], StringSplitOptions.RemoveEmptyEntries) |> Array.map parseField |> Array.toList

let parseHeight (heightStr:string) = 
    let groups = Regex.Match(heightStr, "(\d+)(cm|in)").Groups    
    match groups.[2].Value with
        | "cm" -> Centimeters(int groups.[1].Value)
        | "in" -> Inches(int groups.[1].Value)
        
let validateField field = 
    try
        let name, value = field.Name, field.Value
        match name with 
            | "byr" -> let year = int value 
                       year >= 1920 && year <= 2002
            | "iyr" -> let year = int value
                       year >= 2010 && year <= 2020
            | "eyr" -> let year = int value
                       year >= 2020 && year <= 2030
            | "hgt" -> match parseHeight value with
                            | Centimeters(cm) when cm >= 150 && cm <= 193 -> true
                            | Inches(inch) when inch >= 59 && inch <= 76 -> true                            
            | "hcl" -> Regex.IsMatch(value, "#[0-9a-f]{6}")        
            | "ecl" -> Regex.IsMatch(value, "amb|blu|brn|gry|grn|hzl|oth")
            | "pid" -> value.Length = 9 && value |> Seq.forall Char.IsNumber
    with
        | _ -> false 

let entries = (File.ReadAllText "input.txt").Split "\n\n"
let passports = entries |> Array.map parsePassport

let requiredFields passport = passport |> Seq.filter (fun field -> field.Name <> "cid")
let validatePartOne passport = passport |> requiredFields |> Seq.length = 7
let validatePartTwo passport = passport |> requiredFields |> Seq.filter validateField |> Seq.length = 7

printfn "Part 1: %i" (passports |> Seq.filter validatePartOne |> Seq.length)
printfn "Part 2: %i" (passports |> Seq.filter validatePartTwo |> Seq.length)