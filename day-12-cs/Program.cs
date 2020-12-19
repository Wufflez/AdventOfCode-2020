using System;
using System.IO;
using System.Linq;

var instructions = File.ReadAllLines("input.txt")
    .Select(l => (l[0], int.Parse(l[1..]))).ToList();

(int x, int y) Navigate(bool useWaypoint)
{
    var position = (x: 0, y: 0);
    var waypoint = useWaypoint ? (x:10, y:1) : (x: 1, y: 0);

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

    ref var move = ref useWaypoint ? ref waypoint : ref position;

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
            case ('N', int distance): move.y += distance; break;
            case ('E', int distance): move.x += distance; break;
            case ('S', int distance): move.y -= distance; break;
            case ('W', int distance): move.x -= distance; break;
        }
    }
    return position;
}

var partOnePos = Navigate(false);
Console.WriteLine($"Part 1: {Math.Abs(partOnePos.x) + Math.Abs(partOnePos.y)}");

var partTwoPos = Navigate(true);
Console.WriteLine($"Part 2: {Math.Abs(partTwoPos.x) + Math.Abs(partTwoPos.y)}");