using System;
using System.Collections;
using System.IO;

namespace HackAssembler2 {
    public class Assembler {

// ********** MainProgram Module **********
        string inputFile;
        string outputFile;

        StreamReader SR;
        StreamWriter SW;

        public Assembler(string fileName) {
            // File Format이 .asm인지 검사
            if(!fileName.EndsWith(".asm")) {
                Console.WriteLine("Wrong File Format");
                return;
            }
            
            inputFile = fileName;
            outputFile = String.Copy(fileName).Replace(".asm", ".hack");
            SR = new StreamReader(inputFile);
            SW = new StreamWriter(outputFile);

            string stringToWrite;

            Console.WriteLine("Start");
            while(HasMoreCommands()) {
                Advance();
                switch(CommandType()) {
                    case 'A':
                        stringToWrite = Symbol();
                        SW.WriteLine(stringToWrite);
                        break;
                        
                    case 'C':
                        stringToWrite = ("111" + code_comp() + code_dest() + code_jump());
                        SW.WriteLine(stringToWrite);
                        break;
                        
                    case 'L':
                        break;
                        
                    default:
                        break;
                }
            }

            SR.Close();
            SW.Close();
            Console.WriteLine("End");
        }

// ********** Parser Module **********
        // 현재 처리중인 line을 저장하는 변수 선언
        string line;

        public bool HasMoreCommands() {
            if(SR.Peek() > -1) { // StreamReader 의 Peek 함수 는 뒤에 line이 더 있는지 검사를 수행한다.
                return true;
            }
                return false;
        }

        public void Advance() {
            // 뒤에 Command가 있을 때 수행해야 한다.
            if(HasMoreCommands()) { 
                line = RemoveSpaceComment(SR.ReadLine());
            }
        }

            // 공백 제거를 위한 메서드
        string RemoveSpaceComment(string input) {
            return input.Replace(" ", "").Replace("    ", "").Split("//")[0];
        }
        
        public char CommandType() {
                // 주석인 경우 또는 빈 줄인 경우
            if (line.StartsWith("//") || line.Length == 0) {
                return 'X';
            } 
                // @로 시작하는 경우
            else if(line[0]=='@') {
                return 'A';
            }
                // (로 시작하는 경우
            else if (line[0]=='(') {
                return 'L';
            }
                // 그 외
            else {
                return 'C';
            }
        }

        public string Symbol() {
            char[] sym;
            
            if(CommandType()=='A') {
                sym = new char[line.Length-1];    
                                // Symbol 앞에 @ 이 추가되었으므로, Symbol의 길이는 명령어의 Length-1
                line.CopyTo(1, sym, 0, line.Length-1); // Symbol만 Copy해오기

                //Value 인 경우 16자리 2진수 string으로 반환
                if('0' <= sym[0] && sym[0] <= '9') {
                    return Convert.ToString(Convert.ToInt32(String.Join("", sym)),2).PadLeft(16, '0');
                } 
                //Symbol 인 경우
                else {
                        // 지금은 기호가 없는 hack만 처리한다!
                }
                
            } 
            else if(CommandType()=='L') {
                // 지금은 기호가 없는 hack만 처리한다!
            }
            
            // 제외되는 경우 없음
            return null;
        } 

        public string Dest() {
            int equalIndex = line.IndexOf('=');
            
            // dest는 = 이 있을 때만 존재한다
            // = 이 없는 경우 생략

            if(equalIndex < 0){
                return "000";
            }

            char[] dest = new char[equalIndex];
            line.CopyTo(0, dest, 0, equalIndex);
            
            return String.Join("",dest);
        }

        public string Comp() {
                // '=' 와 ';'의 유무에 따라 dest, jump의 유무가 결정된다.
            int equalIndex = line.IndexOf('=');
            int colonIndex = line.IndexOf(';');
            
            char[] comp;
            
                // colon이 없는 경우 ( D=M-D 같은 경우 )
            if(colonIndex < 0) {
                comp = new char[line.Length-1 - equalIndex]; // equal 뒷부분만 가져오기
                line.CopyTo(equalIndex+1, comp, 0, line.Length-1-equalIndex);
                return String.Join("", comp);
            }
            
                // equal이 없는 경우 ( D;JMP 같은 경우 )
            if(equalIndex < 0) {
                comp = new char[colonIndex];    // colon 앞부분만 가져오기
                line.CopyTo(0, comp, 0, colonIndex);
                return String.Join("", comp);
            }

                // 둘 다 있는 경우는 없다.
            return null;
        }     

        public string Jump() {
            int colonIndex = line.IndexOf(';');
            char[] jump = new char[line.Length-colonIndex-1];

                // colon이 있을 때만 jump가 존재한다.
            if(colonIndex >= 0) {
                line.CopyTo(colonIndex+1, jump, 0, line.Length-1-colonIndex);
            }
            
            return String.Join("", jump);
        }   

// ********** Code Module **********
        public string code_dest() {
            string destString = Dest();
            char[] result = new char[3] {'0','0','0'};
                
            // 각 Bit에 대해 할당된 Destination의 유무에 따라 Bit Set
            if(destString.IndexOf('M') >= 0) {
                result[2] = '1';
            }
            if(destString.IndexOf('D') >= 0) {
                result[1] = '1';
            }
            if(destString.IndexOf('A') >= 0) {
                result[0] = '1';
            }
            
            return new string(result);
        }

        public string code_comp() {
            string compString = Comp();
            switch(compString) {
                case "0":
                    return "0101010";
                    
                case "1":
                    return "0111111";
                    
                case "-1":
                    return "0111010";
                
                case "D":
                    return "0001100";
                    
                case "A" :
                    return "0110000";
                    
                case "M" :
                    return "1110000";
                
                case "!D" : 
                    return "0001101";
                    
                case "!A" : 
                    return "0110001";
                    
                case "!M" :
                    return "1110001";
                    
                case "-D" :
                    return "0001111";
                    
                case "-A" :
                    return "0110011";
                    
                case "-M" :
                    return "1110011";
                    
                case "D+1" :
                    return "0011111";
                
                case "A+1" :
                    return "0110111";
                    
                case "M+1" :
                    return "1110111";
                    
                case "D-1" :
                    return "0001110";
                    
                case "A-1" : 
                    return "0110010";
                    
                case "M-1" :
                    return "1110010";
                    
                case "D+A" :
                    return "0000010";
                    
                case "D+M" :
                    return "1000010";
                    
                case "D-A" :
                    return "0010011";
                    
                case "D-M" :
                    return "1010011";
                    
                case "A-M" :
                    return "0000111";
                    
                case "M-D" :
                    return "1000111";
                    
                case "D&A" :
                    return "0000000";
                    
                case "D&M" :
                    return "1000000";
                    
                case "D|A" :
                    return "0010101";
                
                case "D|M" :
                    return "1010101";
                    
                default:
                    return "0000000";
            }
        }

        public string code_jump() {
            string jumpString = Jump();
            switch(jumpString) {
                case "":
                    return "000";
                    
                case "JGT":
                    return "001";
                    
                case "JEQ":
                    return "010";
                    
                case "JGE":
                    return "011";
                    
                case "JLT":
                    return "100";
                    
                case "JNE":
                    return "101";
                    
                case "JLE":
                    return "110";

                case "JMP":
                    return "111";
                    
                default:
                    return "000";
            }
        }

        
    }
}