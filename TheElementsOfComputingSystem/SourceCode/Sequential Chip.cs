using System;

namespace SequentialChips {
    public static class CLK {
        public delegate void CLKEventHandler();
        public static event CLKEventHandler CLKEvent;

        public static void CLKChange() {
            CLKValue = !(CLKValue);
            if(CLKEvent != null)
            {
                // Console.WriteLine("Event 발생");
                CLKEvent();
            }
        }  

        static bool CLKValue = false;
    }


    // DFF
    // DFF는 NAND 게이트로만 쉽게 구현이 가능하다. 
    // 그러나 전자회로와는 다르게 컴퓨터에서의 신호는 단발성 신호이므로 (지속적으로 신호가 오지 않으므로) 회로 프로그램을 사용하지 않는 이상, Data Flip Flop을 구현하더라도 이전 시간의 신호를 내보내는 DFF는 유효하지 않다고 생각된다. 오히려 이전 신호를 "저장"하기 때문에 이는 Register의 기능이라고 판단. 이에 따라 DFF 대신 들어간 신호를 저장하는 1BitRegister를 적극 활용할 생각이다.
    // 프로젝트의 교재에서도 DFF는 기본 게이트로 생각하고 구현하지는 않는다.

    // 1BitRegister
    public class Bit {
        public bool load = false;

        private bool state;
        private bool inputState;

        public bool value {
            get {
                return state;
            }
            set {
                load = true;
                inputState = value;
            }
        }

        // Constructor
        public Bit() {
            state = false;
            CLK.CLKEvent += new CLK.CLKEventHandler(StateChange);
        }

        // CLK 변화 Event에 반응
        private void StateChange() {
            if(load) {
                // Console.WriteLine("Load 발생");
                state = inputState;    
                load = false;
            }
        }
    }

    // Register
    public class Register {
        private Bit[] state;
        private bool[] inputState;

        public Register() {
            state = new Bit[16];
            for(int i = 0; i < 16; i++) {
                state[i] = new Bit();
            }
       }

        public bool[] value {
            get {
                bool[] result = new bool[16];

                for(int i = 0; i < 16; i++) {
                    result[i] = state[i].value;
                }
                return result;
             }
            set {
                for(int i = 0; i < 16; i++) {
                    state[i].value = value[i];
                }
            }
        }
    }
        //Memory bank
    public class RAM8 {
        private Register[] data;
        private bool[] inputData;

        public RAM8() {
            // 더 큰 RAM을 만들때는 아래 ARRAY만 바꾸어주면 된다.
            data = new Register[8];
            for(int i = 0; i < data.Length; i++) {
                data[i] = new Register();
            }
        }

        public void SetData(bool[] input, bool[] address) {
            data[BoolToInt(address)].value = input;
        }

        public bool[] GetData(bool[] address) {
            return (bool[])data[BoolToInt(address)].value.Clone();
        }

        private int BoolToInt(bool[] input) {
            int result = 0;
            for(int i = input.Length - 1; i >= 0; i--) {
                result = result << 1;
                if(input[i]) result++;
            }
            return result;
        }
    }

        //counter sequentialChips
    public class Counter16Bit {
        private bool reset;
        private bool load;
        private bool inc;

        private Register state;
        private bool[] inputState;
        
        private bool[] zero = new bool[16] {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
        private bool[] one = new bool[16] {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

        public bool[] value {
            get {
                return state.value;
             }
            set {
                inputState = (bool[]) value.Clone();
            }
        }

        // Constructor
        public Counter16Bit() {
            CLK.CLKEvent += new CLK.CLKEventHandler(CLKAction);
            state = new Register();
        }

        // CLK 변화 Event에 반응
        private void CLKAction() {
            if(reset) {
                state.value = zero;
                reset = false;
            }
            else if (load) {
                state.value = inputState;
                load = false;
            } 
            else if (inc) {
                state.value = CombinationalChips.BoolOperation.Add16(state.value, one);
            }
        }

        public void ResetTrue() {
            reset = true;
        }

        public void LoadTrue() {
            load = true;
        }

        public void IncTrue() {
            inc = true;
        }

        public void IncFalse() {
            inc = false;
        }
    }
}