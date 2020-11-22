using System;
using System.Collections;
using System.IO;

namespace HackAssembler{

    public class Assembler{
        string inputFile;
        string outputFile;
        
        StreamReader SR;
        StreamWriter SW;
        string line = "";
        int lineAddress = 0;
        int RAMAddress = 16;
        
        SymbolHashtable symHT;
        
        public Assembler(string fileName) {            
            if(!fileName.EndsWith(".asm")) {
                Console.WriteLine("Input is not a Hack File");
                return;
            }
            
            inputFile = fileName;
            outputFile = (String.Copy(fileName)).Replace(".asm", ".hack");
            Console.WriteLine(inputFile + " " + outputFile);
            
            SR = new StreamReader(inputFile);
            SW = new StreamWriter(outputFile); 
            
            symHT = new SymbolHashtable();

            string stringToWrite;

                    Console.WriteLine("Start");
            // 1 Pass → Construct Hash Table 
            while(HasMoreCommands()) {
                Advance();
                AddressIncrement();
                
                if(CommandType() == 'L') {
                    AddSymbolToHashtable();
                }
            }
            
            ResetStreamReader();
            // 2 Pass → Write ASM Code
            while(HasMoreCommands()) {
                Advance();
                switch(CommandType()) {
                    case 'A':
                        stringToWrite = Symbol();
                        WriteLine(stringToWrite);
                        break;
                        
                    case 'C':
                        stringToWrite = ("111" + code_comp() + code_dest() + code_jump());
                        WriteLine(stringToWrite);
                        break;
                        
                    case 'L':
                        break;
                        
                    default:
                        break;
                }
            }
            CloseStream();
            Console.WriteLine("End");
        }
        
        public void ResetStreamReader() {
            SR.Close();
            SR = new StreamReader(inputFile);
        }
        
        public void CloseStream() {
            SR.Close();
            SW.Close();
        }
        
        public void WriteLine(string stringToWrite) {
            SW.WriteLine(stringToWrite);
        }

        
// Parser module --------------------------

        // hasMoreCommands
        // 다음 입력이 있는지 확인
        public bool HasMoreCommands() {
            if(SR.Peek() > -1) {
                return true;
            }
            return false;
        }
        
        // advance
        // 다음 명령을 읽어 현재 명령으로 만듦
        public void Advance() {
            if(HasMoreCommands()) {
               line = SR.ReadLine().Replace(" ","").Replace("   ","").Split("//")[0];
            }
        }
        
        // CommandType
        // 현재 명령의 명령 type을 반환 (A/C/L)
        public char CommandType() {
            if (line.StartsWith("//") || line.Length == 0) {
                return 'X';
            } 
            else if(line[0]=='@') {
                return 'A';
            }
            else if (line[0]=='(') {
                return 'L';
            }
            else {
                return 'C';
            }
        }
        
        
        // Symbol
        // A/L type의 명령어에서 기호를 반환
        public string Symbol() {
            char[] sym;
            string symString;
            
            if(CommandType()=='A') {
                sym = new char[line.Length-1];
                
                line.CopyTo(1, sym, 0, line.Length-1);
                //Value 인 경우
                if('0' <= sym[0] && sym[0] <= '9') {
                    return Convert.ToString(Convert.ToInt32(String.Join("", sym)),2).PadLeft(16, '0');
                } 
                //Symbol 인 경우
                else {
                    symString = String.Join("", sym).Trim();
                    
                        // HashTable에 있으면 생략
                    if(symHT.Contains(symString)) {
                        return Convert.ToString(symHT.GetAddress(symString),2).PadLeft(16, '0');
                    }
                    else {
                        // HashTable에 없으면 추가
                        symHT.AddEntry(symString, RAMAddress++);
                        return Convert.ToString(symHT.GetAddress(symString),2).PadLeft(16, '0');
                    }
                }
                
                
            } 
            else if(CommandType()=='L') {
                sym = new char[line.Length-2];
                line.CopyTo(1, sym, 0, line.Length-2);
                
                return String.Join("", sym);
            }
            
            // 제외되는 경우 없음
            return null;
        } 
        
        // Dest
        // C 명령어의 dest 연상기호 반환
        public string Dest() {
            
            int equalIndex = line.IndexOf('=');
            
            // = 이 없는 경우 생략
            if(equalIndex < 0) {
                return "";
            }
            
            char[] dest = new char[equalIndex];
            
            if(equalIndex >= 0) {
                line.CopyTo(0, dest, 0, equalIndex);
            }
            else {
                return "";
            }
            
            return String.Join("",dest);
        }
        
        // Comp
        // C 명령어의 comp 연상기호 반환
        public string Comp() {
            int equalIndex = line.IndexOf('=');
            int colonIndex = line.IndexOf(';');
            
            char[] comp;
            
            if(colonIndex < 0) {
                comp = new char[line.Length-1 - equalIndex];
                line.CopyTo(equalIndex+1, comp, 0, line.Length-1-equalIndex);
                return String.Join("", comp);
            }
            
            if(equalIndex < 0) {
                comp = new char[colonIndex];
                line.CopyTo(0, comp, 0, colonIndex);
                return String.Join("", comp);

            }
            
            return null;
            
        }
        
        // Jump
        // C 명령어의 jump 연상기호 반환
        public string Jump() {
            int colonIndex = line.IndexOf(';');
            char[] jump = new char[line.Length-colonIndex-1];

            if(colonIndex >= 0) {
                line.CopyTo(colonIndex+1, jump, 0, line.Length-1-colonIndex);
            }
            
            return String.Join("", jump);
        }
        
        
// Code module --------------------------
        public string code_dest() {
            string destString = Dest();
            char[] result = new char[3] {'0','0','0'};
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
        
        //Symbol Table
                
        public void AddressIncrement() {
            if(CommandType()=='A' || CommandType()=='C') {
                lineAddress++;
            }
            return;
        }
        
        public void AddSymbolToHashtable() {
            if(!symHT.Contains(Symbol())) {
                symHT.AddEntry(Symbol(),lineAddress);    
            }
            return;
        }
    }    
    
    public class SymbolHashtable {
        Hashtable symbolHT;
        
        public SymbolHashtable() {
            symbolHT = new Hashtable();
            symbolHT.Add("SP", 0);
            symbolHT.Add("LCL", 1);
            symbolHT.Add("ARG", 2);
            symbolHT.Add("THIS", 3);
            symbolHT.Add("THAT", 4);
            symbolHT.Add("R0", 0);
            symbolHT.Add("R1", 1);
            symbolHT.Add("R2", 2);
            symbolHT.Add("R3", 3);
            symbolHT.Add("R4", 4);
            symbolHT.Add("R5", 5);
            symbolHT.Add("R6", 6);
            symbolHT.Add("R7", 7);
            symbolHT.Add("R8", 8);
            symbolHT.Add("R9", 9);
            symbolHT.Add("R10", 10);
            symbolHT.Add("R11", 11);
            symbolHT.Add("R12", 12);
            symbolHT.Add("R13", 13);
            symbolHT.Add("R14", 14);
            symbolHT.Add("R15", 15);
            symbolHT.Add("SCREEN", 16384);
            symbolHT.Add("KBD", 24576);
            
        }
        
        public void AddEntry(string symbol, int address) {
            symbolHT.Add(symbol, address);
        }
        
        public bool Contains(string symbol) {
            return (symbolHT.Contains(symbol));
        }
        
        public int GetAddress(string symbol) {
            return Int32.Parse(symbolHT[symbol].ToString());   
        }
    }
}
