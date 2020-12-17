using System;
using System.Linq;

var input = System.IO.File.ReadLines("input.txt").Select(long.Parse).ToArray();

long[] FindSumSequence(long[] data, long target) {
    int last = 0, first = 0;
    long total = data[0];
    while (total != target)
        if (total < target)
        {
            last += 1;
            total += data[last];
        }
        else
        {
            total -= data[first];
            first += 1;
        }

    return data[first..(last+1)];
}

var sequence = FindSumSequence(input, 70639851);
long p2Answer = sequence.Min() + sequence.Max();
Console.WriteLine($"Part 2 = {p2Answer}");
