using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

bool EntryValid(string entry, bool useCounts)
{
    var parts = Regex.Match(entry, @"(\d+)+-(\d+)\s(\w):\s(.*)").Groups;
    int a = int.Parse(parts[1].Value);
    int b = int.Parse(parts[2].Value);
    char letter = parts[3].Value[0];
    if (!useCounts) 
        return parts[4].Value[a - 1] == letter ^ parts[4].Value[b - 1] == letter;
    int count = parts[4].Value.Count(ch => ch == letter);
    return count >= a && count <= b;
}

int part1 = File.ReadLines("input.txt").Count(e => EntryValid(e, true));
Console.WriteLine("Part1: " + part1);

int part2 = File.ReadLines("input.txt").Count(e => EntryValid(e, false));
Console.WriteLine("Part2: " + part2);