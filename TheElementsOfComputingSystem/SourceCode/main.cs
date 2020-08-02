using System;
using CombinationalChips;
using DebugTools;

class MainClass {
    public static void Main (string[] args) {
        while(true) {
            Console.Write("VA : ");
            int a = int.Parse(Console.ReadLine());
            Console.Write("VB : ");
            int b = int.Parse(Console.ReadLine());

            bool[] input = DebugTool.IntToBools16(a);
            bool[] input2 = DebugTool.IntToBools16(b);
            
            bool[] outValue = new bool[2];
            
            Console.Write("Flag : ");
            int flags = int.Parse(Console.ReadLine());
            bool[] setValue = DebugTool.IntToBoolFlagReverse6(flags);


            bool[] result = BoolOperation.ALU(input, input2, 
                                            setValue[0], setValue[1], setValue[2], setValue[3], setValue[4], setValue[5], 
                                            out outValue[0], out outValue[1]);

            DebugTool.PrintBools(result);
            Console.WriteLine("zr = {0}, ng = {1}\n", outValue[0], outValue[1]);
        }
    }
}

