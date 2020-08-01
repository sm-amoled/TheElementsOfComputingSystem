using System;
using System.Linq;  // Enumerable 사용을 위함

namespace CombinationalChips {
    
    // Chapter 2 불 논리
    class BoolLogic {
        // Nand GATE는 OR, And 게이트보다 구현하는데 있어서 더 적은 부품을 사용하며 속도도 더 빠르고 집적율도 좋으며 어떤 논리회로도 Nand GATE 만으로 그현할 수 있기 때문에, Nand GATE를 기본 소자로 하여 칩을 디자인한다.

        public static bool Nand(bool a, bool b) {
            return !(a&b);
        }

        public static bool And(bool a, bool b) {
            return Nand(Nand(a,b),Nand(a,b));
        } 

        public static bool Not(bool a) {
            return Nand(a,a);
        }
        
        public static bool Or(bool a, bool b) {
            return Nand(Nand(a,a),Nand(b,b));
        }

        public static bool Xor(bool a, bool b) {
            return Nand(Nand(a,Nand(b,b)),Nand(Nand(a,a),b));
        }

        public static bool Mux(bool a, bool b, bool selector) {
            return Nand(Nand(Nand(Nand(a,selector),Nand(a,selector)) ,Nand(Nand(a,selector),Nand(a,selector)) ), Nand(Nand(Nand(b,Nand(selector, selector)),Nand(b,Nand(selector,selector))),Nand(Nand(b,Nand(selector, selector)),Nand(b,Nand(selector,selector)))));
        }

        // DMUX를 사용할 때는 operands 에 out keyword를 붙여줘야한다.
        public static void Dmux(bool input, out bool a, out bool b, bool selector) {
            a = Nand(Nand(input,selector),Nand(input,selector));
            b = Nand(Nand(input,Nand(selector, selector)),Nand(input,Nand(selector, selector)));
            return;           
        }

        //*** Multi Bit Gates *********************
        // 여기에서부터는 굳이 NAND만을 이용하여 게이트를 만들지 않을 예정!
        // Array를 가져온 그대로 return 해주거나 단순히 포인터만 옮겨서 반환하면 얕은 복사가 되어 값이 꼬일 가능성이 있다.
        // 실제 HW에서는 출력값의 변화가 입력값에 영향을 주지 않는 순차적인 방식이 적용되므로, 새로운 배열을 만들어 return 해줬다.

        public static bool[] Not16(bool[] input) {
            bool[] output = input;
            
            for(int i = 0; i < 16; i++) {
                output[i] = Not(output[i]);
            }

            return output;
        }

        public static bool[] And16(bool[] A, bool[] B) {
            bool[] output = new bool[16];

            for(int i = 0; i < 16; i++) {
                output[i] = And(A[i], B[i]);
            }

            return output;
        }

        public static bool[] Or16(bool[] A, bool[] B) {
            bool[] output = new bool[16];


            for(int i = 0; i < 16; i++) {
                output[i] = Or(A[i], B[i]);
            }

            return output;
        }

        public static bool[] Mux16(bool[] A, bool[] B, bool selector) {
            bool[] output = new bool[16];
            bool[] selected = (selector)? A:B;

            for(int i = 0; i <16; i++) {
                output[i] = selected[i];
            }
            
            return output;
        }

        //*** Multi Way Gate *************
        public static bool Or8Way(bool[] input) {
            return Or(Or(Or(input[0], input[1]), Or(input[3], input[4])),Or(Or(input[5], input[6]), Or(input[7], input[8])));
        }

        public static bool[] Mux4Way16(bool[] A, bool[] B, bool[] C, bool[] D, bool[] selector) {
            bool[] output = new bool[16];
            bool[] selected = (selector[1])? ((selector[0])? D:C):((selector[0])? B:A);

            for(int i = 0; i <16; i++) {
                output[i] = selected[i];
            }

            return output;
        }

        public static bool[] Mux8Way16(bool[] A, bool[] B, bool[] C, bool[] D, bool[] E, bool[] F, bool[] G, bool[] H, bool[] selector) {
            bool[] output = new bool[16];
            bool[] selected = (selector[2])? (selector[1])? ((selector[0])? H:G) : ((selector[0])? F:E) : (selector[1])? ((selector[0])? D:C) : ((selector[0])? B:A);

            for(int i = 0; i <16; i++) {
                output[i] = selected[i];
            }

            return output;
        }

        public static void DMux4Way (bool[] input, out bool[] A, out bool[] B, out bool[] C, out bool[] D, bool[] selector) {
            //CS0269 out관련 에러 대응 초기화
            A = input; B = input; C = input; D = input;

            A = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            B = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            C = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            D = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();

            int selectSwitch = 0;
            for(int i = 0; i < selector.Length; i++) {
                selectSwitch += (Convert.ToInt32(selector[i]) * (int)(Math.Pow(2,i)));
            }

            switch(selectSwitch) {
                case 0:
                    A = (bool[]) input.Clone();
                    break;
                case 1:
                    B = (bool[]) input.Clone();
                    break;
                case 2:
                    C = (bool[]) input.Clone();
                    break;
                case 3:
                    D = (bool[]) input.Clone();
                    break;
            }

            return;
        }

        public static void DMux8Way (bool[] input, out bool[] A, out bool[] B, out bool[] C, out bool[] D, out bool[] E, out bool[] F, out bool[] G, out bool[] H, bool[] selector) {
            //CS0269 out관련 에러 대응 초기화
            A = input; B = input; C = input; D = input; E = input; F = input; G = input; H = input;

            A = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            B = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            C = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            D = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            E = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            F = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            G = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();
            H = Enumerable.Repeat<bool>(false, A.Length).ToArray<bool>();

            int selectSwitch = 0;
            for(int i = 0; i < selector.Length; i++) {
                selectSwitch += (Convert.ToInt32(selector[i]) * (int)(Math.Pow(2,i)));
            }
                Console.WriteLine(selectSwitch);

            switch(selectSwitch) {
                case 0:
                    A = (bool[]) input.Clone();
                    break;
                case 1:
                    B = (bool[]) input.Clone();
                    break;
                case 2:
                    C = (bool[]) input.Clone();
                    break;
                case 3:
                    D = (bool[]) input.Clone();
                    break;
                case 4:
                    E = (bool[]) input.Clone();
                    break;
                case 5:
                    F = (bool[]) input.Clone();
                    break;
                case 6:
                    G = (bool[]) input.Clone();
                    break;
                case 7:
                    H = (bool[]) input.Clone();
                    break;
            }

            return;
        }
    }

    // Chapter 2 불 연산
    class BoolOperation {
        public static void HalfAdder(bool a, bool b, out bool sum, out bool carry) {
            sum = BoolLogic.Xor(a,b);
            carry = BoolLogic.And(a,b);
        }

        static bool HalfAdder_Carry(bool a, bool b) {
            return BoolLogic.And(a,b);
        }

        static bool HalfAdder_Sum(bool a, bool b) {
            return BoolLogic.Xor(a,b);
        }

        public static void FullAdder(bool a, bool b, bool c, out bool sum, out bool carry) {
            carry = BoolLogic.Or(HalfAdder_Carry(a,b), HalfAdder_Carry(HalfAdder_Sum(a,b), c));
            sum = HalfAdder_Sum(HalfAdder_Sum(a,b), c);
        }

        static bool FullAdder_Carry(bool a, bool b, bool c) {
            return BoolLogic.Or(HalfAdder_Carry(a,b), HalfAdder_Carry(HalfAdder_Sum(a,b), c));
        }

        static bool FullAdder_Sum(bool a, bool b, bool c) {
            return HalfAdder_Sum(HalfAdder_Sum(a,b), c);
        }

        public static bool[] Add16(bool[] a, bool[] b) {
            bool[] result = new bool[16];
            bool carry = false;

            for(int i = 0; i < 16; i++) {
                result[i] = FullAdder_Sum(a[i], b[i], carry);
                carry = FullAdder_Carry(a[i], b[i], carry);
            }

            return result;
        }

        public static bool[] Inc16(bool[] input) {
            bool[] one = new bool[16] {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
            
            return Add16(input, one);
        }
    }
}