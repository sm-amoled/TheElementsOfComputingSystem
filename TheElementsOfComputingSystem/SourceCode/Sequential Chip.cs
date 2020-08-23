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
    public class DFF {
        private bool state;
        private bool inputState;

        public bool value {
            get {
                return state;
            }
            set {
                inputState = value;
            }
        }

        public DFF() {
            state = false;
            CLK.CLKEvent += new CLK.CLKEventHandler(StateChange);
        }

        private void StateChange() {
            state = inputState;
        }
    }


    // 1BitRegister
    public class Bit {
        private DFF dff;

        public bool value {
            get {
                return dff.value;
            }
            set {
                // 새로운 값이 들어오면 load bit이 1이라고 가정하고 값을 바꾸어준다. 
                // 그게 아니라면 값을 유지.
                dff.value = value;
            }
        }

        // Constructor
        public Bit() {
            dff = new DFF();
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
