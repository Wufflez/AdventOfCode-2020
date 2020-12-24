using System;
using System.Linq;

var lines = System.IO.File.ReadAllLines("input.txt");
int startTs = int.Parse(lines[0]);
var busses = lines[1]
    .Split(",")
    .Select((busId, index) => (index, busId))
    .Where(item => item.busId != "x")
    .Select(e => (e.index, bus: long.Parse(e.busId)))
    .ToArray();

var firstBus = busses.Select(e => (e.bus, waitTime: e.bus - startTs % e.bus)).OrderBy(e => e.waitTime).First();
Console.WriteLine("Part 1: " + firstBus.bus * firstBus.waitTime);

var value = busses.First().bus;
var increment = value;

foreach (var (index, bus) in busses.Skip(1))
{
    var mod = (-index % bus + bus) % bus;
    Console.WriteLine("mod = {0}", mod);
    while (value % bus != mod) 
        value += increment;
    increment *= bus;
}

Console.WriteLine("Part 2: " + value);