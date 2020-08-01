using System;
using CombinationalChips;
using DebugTools;

class MainClass {
    public static void Main (string[] args) {
       
        bool[] input = DebugTool.IntToBools(7);
        bool[] input2 = DebugTool.IntToBools(14);
        
        bool[] result = BoolOperation.Inc16(input);
        DebugTool.PrintBools(result);

    }
}
