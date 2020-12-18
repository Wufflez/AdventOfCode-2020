using System;
using System.IO;
using System.Linq;

bool ValidField(string field) 
{
    var parts = field.Split(':'); // field in the form "id:data"
    var (id, data) = (parts[0], parts[1]);
    return id switch // Each field id has own data rules - see AoC question for more details
    {
        "byr" => int.Parse(data) is >= 1920 and <= 2002,
        "iyr" => int.Parse(data) is >= 2010 and <= 2020,
        "eyr" => int.Parse(data) is >= 2020 and <= 2030,
        "hgt" => data.Length > 2 && (int.Parse(data[..^2]), data[^2..]) switch
        {
            (>= 150 and <= 193, "cm") => true,
            (>= 59 and <= 76, "in") => true,
            _ => false,
        },
        "hcl" => data.First() == '#' && data.Skip(1).All(char.IsLetterOrDigit),
        "ecl" => data is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth",
        "pid" => data.Length == 9 && data.All(char.IsDigit),
        _ => throw new ArgumentException("Invalid field ID"),
    };
}

// Each passport separated by '\n\n', fields separated by '\n' or ' '
var passports = File.ReadAllText("input.txt")
     .Split("\n\n")       
     .Select(passport=>passport.Trim().Split('\n', ' ')).ToList();

bool IsRequired(string field) => !field.StartsWith("cid");  // A field is required if it doesn't start with "cid"

// Part 1: count passports that contain 7 required fields
Console.WriteLine("Part 1: " + passports.Count(fields => fields.Count(IsRequired) == 7));

// Part 2: count passports that contain 7 required fields that are also valid
Console.WriteLine("Part 2: " + passports.Count(fields => fields.Where(IsRequired).Count(ValidField) == 7));