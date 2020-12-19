using System;
using System.IO;
using System.Linq;

var instructions = File.ReadAllLines("input.txt")
    .Select(l => (l[0], int.Parse(l[1..]))).ToList();

(int x, int y) Navigate()
{
    int angle = 0;
    var position = (x: 0, y: 0);

    void Move((int x, int y) vector)
    {
        position.x += vector.x;
        position.y += vector.y;
    }

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
    return position;
}

(int x, int y) NavigateWithWaypoint()
{
    var position = (x: 0, y: 0);
    var waypoint = (x: 10, y: 1);

    void RotateWaypoint(int angle)
    {
        waypoint = ((angle + 360) % 360) switch
        {
            0 => waypoint,
            90 => (waypoint.y, -waypoint.x),
            180 => (-waypoint.x, -waypoint.y),
            270 => (-waypoint.y, waypoint.x),
        };
    }
    foreach (var instruction in instructions)
    {
        switch (instruction)
        {
            case ('R', int angle): RotateWaypoint(angle); break;
            case ('L', int angle): RotateWaypoint(-angle); break;
            case ('F', int times):
                position.x += waypoint.x * times;
                position.y += waypoint.y * times;
                break;
            case ('N', int distance): waypoint.y += distance; break;
            case ('E', int distance): waypoint.x += distance; break;
            case ('S', int distance): waypoint.y -= distance; break;
            case ('W', int distance): waypoint.x -= distance; break;
        }
    }
    return position;
}

var partOnePos = Navigate();
Console.WriteLine($"Part 1: {Math.Abs(partOnePos.x) + Math.Abs(partOnePos.y)}");

var partTwoPos = NavigateWithWaypoint();
Console.WriteLine($"Part 2: {Math.Abs(partTwoPos.x) + Math.Abs(partTwoPos.y)}");