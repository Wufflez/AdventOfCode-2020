using System;
using System.IO;
using System.Linq;

var map = File.ReadAllLines("input.txt");
int width = map[0].Length;

long CountTrees((int x, int y) slope)
{
    long count = 0;
    int x = 0;
    for (int y = 0; y < map.Length; y += slope.y)
    {
        if (map[y][x] == '#')
            count++;
        x = (x + slope.x) % width;
    }
    return count;
}

Console.WriteLine("Part 1: " + CountTrees((3, 1)));

var slopes = new[] {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)};
Console.WriteLine("Part 2: " + slopes.Select(CountTrees).Aggregate((x, y) => x * y));
