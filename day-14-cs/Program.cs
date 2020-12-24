using System;
using System.Collections.Generic;
using System.Linq;

var lines = System.IO.File.ReadAllLines("input.txt");

long RunProgram(bool v2 = false)
{
    var mem = new Dictionary<long, long>();
    long alt = 0, set = 0;
    foreach (string line in lines)
    {
        var parts = line.Split(" = ");
        var (cmd, value) = (parts[0], parts[1]);
        if (cmd == "mask")
        {
            alt = 0; // Alt mask (Zero for V1, Floating for V2)
            set = 0; // Set mask (Both versions)
            for (int i = 0; i < 36; i++)
                switch (value[35 - i])
                {
                    case '1':
                        set += (long)1 << i;
                        break;
                    case '0' when !v2:
                    case 'X' when v2:
                        alt += (long)1 << i;
                        break;
                }
        }
        else
        {
            var addr = long.Parse(cmd[4..^1]);
            var num = long.Parse(value);
            if (v2)
            {
                addr |= set;
                var addrs = GetFloatingPerms(alt, addr);
                foreach (var a in addrs) mem[a] = num;
            }
            else
            {
                num = (num | set) & ~alt; // Set & zero from masks.
                mem[addr] = num;
            }
        }
    }
    return mem.Values.Sum();
}


List<long> GetFloatingPerms(long floatingMask, long initialValue)
{
    List<long> combinations = new List<long>{floatingMask | initialValue};
    for (int i = 0; i < 36; i++)
    {
        if ((1L << i & floatingMask) != 0)
            combinations.AddRange(combinations.Select(comb => comb &= ~(1L << i)).ToList());
    }

    return combinations;
}

Console.WriteLine("Part 1: " + RunProgram());
Console.WriteLine("Part 2: " + RunProgram(v2: true));