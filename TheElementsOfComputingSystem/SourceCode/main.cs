using System;
using Gates;

class MainClass {
    public static void Main (string[] args) {
       
        bool[] input = {true, false, false, true, false, false, true, false, false, true, false, false, true, false, false, true};
        bool[] input1 = {true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false};
        bool[] input2 = {false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true};
        bool[] input3 = {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true};
        bool[] input4 = {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
        

        BoolGate.DMux4Way(input, out input1, out input2, out input3, out input4, new bool[] {false, true});
    }
}
