 using System;
 
 namespace DebugTools {
     class DebugTool {
        public static bool[] IntToBools8(int input) {
            bool[] result = new bool[8];

            for(int i = 7; i >= 0; i--) {
                result[7-i] = (input % 2 == 1)? true:false;
                input /= 2;
            }

            return result;
        }

        public static bool[] IntToBoolFlagReverse6(int input) {
            bool[] result = new bool[6];

            for(int i = 5; i >= 0; i--) {
                result[i] = (input % 10 == 1)? true:false;
                input /= 10;
            }

            return result;
        }

        public static bool[] IntToBools16(int input) {
            bool[] result = new bool[16];

             for(int i = 15; i >= 0; i--) {
                result[15-i] = (input % 2 == 1)? true:false;
                input /= 2;
            }

            return result;
        }

        public static bool[] NumbsToBool8(int input) {
            bool[] result = new bool[8];

            for(int i = 0; i < 8; i++) {
                result[i] = (input%10 != 0)? true:false;
                input /= 10;;
            }

            return result;
        }

        public static void PrintBools(bool[] input) {
            int temp;
            for(int i = input.Length-1; i >= 0; i--) {
                temp = (input[i])? 1:0;
                Console.Write("{0}", temp);
            }

            Console.WriteLine("");
            return;
        }
     }
 }
 

