@24576
D=A
@R1
M=D

@SCREEN
D=A-1
@R0
M=D

(FILL)
@0
D = !A	// 1111 1111 1111 1111
@R0
A=M
M=D	// R0가 가진 주소에 (-1)을 넣는다

(KBDINPUT)
@R0
M=M+1
@R1
D=M
@R0
D=D-M
@RESET
D;JLE	// 최대 범위를 넘지 않는지 검사

@R0
A=M
M=0

@KBD
D=M
@FILL
D;JNE	// KBD에서 가져온 값이 0이 아니라면 SCREENFILL

@KBDINPUT
0;JMP

