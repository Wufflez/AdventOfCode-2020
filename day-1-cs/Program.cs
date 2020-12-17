using System;
using System.IO;
using System.Linq;

const int targetSum = 2020;
int[] input = File.ReadLines("input.txt").Select(int.Parse).OrderBy(x=>x).ToArray();

//Part 1
var values = FindSumValues(input.AsSpan(), targetSum, 2);
Console.WriteLine($"Part 1: { values[0] * values[1] }");

// Part 2
values = FindSumValues(input.AsSpan(), targetSum, 3);
Console.WriteLine($"Part 2: { values[0] * values[1] * values[2] }");

static int[] FindSumValues(ReadOnlySpan<int> data, int sum, int n)
{
    foreach (int value in data)
        if (n > 2)
        { 
            if (FindSumValues(data[1..], sum - value, n - 1) is int[] others )
                return new[] { value }.Concat(others).ToArray();
        }
        else
        {
            foreach (int other in data[1..])
                if (value + other == sum)
                    return new[] { value, other };
        }

    return null;
}
