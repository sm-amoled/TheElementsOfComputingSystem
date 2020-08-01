 using System;
 
 namespace DebugTools {
     class DebugTool {
        public static bool[] IntToBools(int input) {
            bool[] result = new bool[16];

            for(int i = 0; i < 16; i++) {
                result[i] = (input%2 == 1)? true:false;
                input /= 2;;
            }

            return result;
        }
        public static void PrintBools(bool[] input) {
            int temp;
            for(int i = input.Length-1; i >= 0; i--) {
                temp = (input[i])? 1:0;
                Console.Write("{0}", temp);
            }

            return;
        }
     }
 }