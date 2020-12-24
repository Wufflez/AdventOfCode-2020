using System;
using System.Collections.Generic;
using System.Linq;

var lines = System.IO.File.ReadAllLines("input.txt");

var mem = new Dictionary<uint, long>();
long zero =0, set = 0;

foreach (string line in lines)
{
    var parts = line.Split(" = ");
    var (cmd, value) = (parts[0], parts[1].Trim());
    if (cmd == "mask") // Update masks command
    {
        zero = 0;
        set = 0;
        for (int i = 0; i < 36; i++)
            switch (value[35 - i])
            {
                case '1':
                    set += (long) 1 << i;
                    break;
                case '0':
                    zero += (long) 1 << i;
                    break;
            }
    }
    else
    {
        var addr = uint.Parse(cmd[4..^1]);
        var num = long.Parse(value);
        num = (num | set) & ~zero; // Set & zero from masks.
        mem[addr] = num;
    }
}

Console.WriteLine("Part 1: " + mem.Values.Sum());