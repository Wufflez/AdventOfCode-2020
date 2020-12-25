using System;
using System.Collections.Generic;
using System.Linq;

static IEnumerable<int> GuessingGame(params int[] startNums)
{
    int position = 0;
    int lastNumber = startNums.Last();
    var positions = new Dictionary<int, int>();

    foreach (var number in startNums)
    {
        positions[number] = ++position;
        yield return number;
    }

    while (true)
    {
        int nextNumber = positions.TryGetValue(lastNumber, out int lastSeenPosition) ? position - lastSeenPosition : 0;
        yield return nextNumber;
        positions[lastNumber] = position++;
        lastNumber = nextNumber;
    }
}

Console.WriteLine("Part 1: " + GuessingGame(0, 3, 1, 6, 7, 5).Take(2020).Last());
Console.WriteLine("Part 2: " + GuessingGame(0, 3, 1, 6, 7, 5).Take(30000000).Last());
