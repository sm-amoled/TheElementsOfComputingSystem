using System;
using CombinationalChips;
using SequentialChips;
using DebugTools;

class MainClass {
    public static void Main (string[] args) {

        Counter16Bit counter = new Counter16Bit();

        bool[] input = DebugTool.IntToBools16(15);
        counter.value = input;

        counter.LoadTrue();
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);

        counter.IncTrue();

        Console.WriteLine("");
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);

        Console.WriteLine("");

        counter.IncFalse();
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);

        Console.WriteLine("");

        counter.ResetTrue();
        counter.IncTrue();
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);

        Console.WriteLine("");

        counter.value = DebugTool.IntToBools16(256);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);

        Console.WriteLine("");

        counter.LoadTrue();
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
        CLK.CLKChange();
        DebugTool.PrintBools(counter.value);
    }
}

