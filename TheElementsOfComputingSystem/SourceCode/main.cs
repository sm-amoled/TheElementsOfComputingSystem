using System;
using CombinationalChips;
using DebugTools;

class MainClass {
    public static void Main (string[] args) {
       
        bool[] input = DebugTool.IntToBools16(1);
        bool[] input2 = DebugTool.IntToBools16(5);

        bool[] setValue = DebugTool.IntToBoolFlagReverse6(010101);
        bool[] outValue = new bool[2];


        bool[] result = BoolOperation.ALU(input, input2, setValue[0], setValue[1], setValue[2], setValue[3], setValue[4], setValue[5], out outValue[0], out outValue[1]);

        DebugTool.PrintBools(result);
        Console.WriteLine(" zr = {0}, ng = {1}", outValue[0], outValue[1]);
    }
}


