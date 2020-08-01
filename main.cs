using System;
using CombinationalChips;
using DebugTools;

class MainClass {
    public static void Main (string[] args) {
       
        bool[] input = DebugTool.IntToBools16(1);
        bool[] input2 = DebugTool.IntToBools16(1);

        bool[] setValue = DebugTool.IntToBools8(01111100);

        bool[] result = BoolOperation.ALU(input, input2, setValue[7], setValue[6], setValue[5], setValue[4], setValue[3], setValue[2], out setValue[1], out setValue[0]);

        DebugTool.PrintBools(result);
    }
}
