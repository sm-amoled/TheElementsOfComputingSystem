using System;
using Gates;

class MainClass {
    public static void Main (string[] args) {
       
        bool[] input1 = {true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false};
        bool[] input2 = {false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true};
        bool[] input3 = {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true};
        bool[] input4 = {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
        

        foreach(bool item in BoolGate.Mux4Way16(input1, input2, input3, input4, new bool[] {false, true})) {
            Console.WriteLine(item + " ");
        }

    }
}
