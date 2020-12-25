using System;
using System.Linq;

static int GuessingGame(int nth, params int[] startNums)
{
    int position = 0;
    int lastNumber = startNums.Last();
    var positions = new int[nth];
    foreach (var number in startNums)
        positions[number] = ++position;

    while (position < nth)
    {
        int lastPosition = positions[lastNumber];
        int nextNumber = lastPosition != 0 ? position - lastPosition : 0;
        positions[lastNumber] = position++;
        lastNumber = nextNumber;
    }

    return lastNumber;
}

Console.WriteLine("Part 1: " + GuessingGame(2020, 0, 3, 1, 6, 7, 5));
Console.WriteLine("Part 2: " + GuessingGame(30000000, 0, 3, 1, 6, 7, 5));
