using System;
using System.IO;
using System.Linq;

var instructions = File.ReadAllLines("input.txt")
    .Select(l => (l[0], int.Parse(l[1..]))).ToList();

int angle = 0;
var position = (x: 0, y: 0);

void Move((int x, int y) vector)
{
    position.x += vector.x;
    position.y += vector.y;
}

void Turn(int degrees) => angle = (angle + degrees + 360) % 360;
foreach (var instruction in instructions)
{
    switch (instruction)
    {
        case ('R', int amount): angle = (angle + amount + 360) % 360; break;
        case ('L', int amount): angle = (angle - amount + 360) % 360; break;
        case ('F', int distance):
            Move(angle switch
            {
                0 => (distance, 0),
                90 => (0, -distance),
                180 => (-distance, 0),
                270 => (0, distance),
            });
            break;
        case ('N', int distance): Move((0, distance)); break;
        case ('E', int distance): Move((distance, 0)); break;
        case ('S', int distance): Move((0, -distance)); break;
        case ('W', int distance): Move((-distance, 0)); break;
    }
}

Console.WriteLine($"Part 1: {Math.Abs(position.x) + Math.Abs(position.y)}");