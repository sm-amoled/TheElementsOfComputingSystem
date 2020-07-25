namespace Gates {
    class BoolGate {
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
    }
}