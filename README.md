# BrainRingAppV2
Вторая версия программы для регистрации нажатий кнопок на Брейн ринге. В отличие от первой версии программы, данная является своеобразным монитором. Все управление устройством осуществляется с помощью тач скрина, который присоединен к устройству.

# Протокол работы работы устройства следующий:

SPEC-BR PC COMMUNICATION PROTOCOL
19200, 8, N, 1 
Period: 1 packet every 200 ms


| N  | Format Description | ASCII Coding | Notes |
|----|--------------------|--------------|-------|
| 0  | SOF                |              |       |
| 1  | HEX DIGIT IN SYSTEM STATE (phases) | ASCII CODING 0 – IDLE (RESET ALL), 1 - REGISTER FALSES, 2 - REGISTER EARLY | #define TFLAG_IDLE, #define TFLAG_FALSES, #define TFLAG_EARLY |
| 2  | HEX DIGIT IN MOST SIGNIFICANT BIT | | |
| 3  | REGISTER OTHER | | |
| 4  | STOPPED | | #define TFLAG_STOPPED |
| 5  | HEX DIGIT IN RING CURRENT TIME (ms) | | |
| 6  | HEX DIGIT IN | | |
| 7  | HEX DIGIT IN RING TOTAL TIME (ms) | | |
| 8  | HEX DIGIT IN | | |
| 9  | HEX DIGIT IN LEAST SIGNIFICANT BIT | | |
| 10 | DELIMITER | | |
| 11 | HEX DIGIT IN BUTTON1 STATE | ASCII CODING 0 - NOT PRESSED, 1 - FALSTART, 2 - PRESSED @ EARLYTIME, 3 - PRESSED @ RINGTIME | #define BUTTON_STATE_IDLE 0, #define BUTTON_STATE_FSTART1, #define BUTTON_STATE_EARLY 2, #define BUTTON_STATE_PRESSED |
| 12 | HEX DIGIT IN BUTTON1 ORDER 0 – 8 | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER | |
| 13 | HEX DIGIT IN | | |
| 14 | HEX DIGIT IN Button 1 PRESS-TIME (ms) | | |
| 15 | HEX DIGIT IN MOST SIGNIFICANT BIT | | |
| 16 | HEX DIGIT IN LEAST SIGNIFICANT BIT | | |
| 17 | DELIMITER | | |
| 18 | HEX DIGIT IN BUTTON2 STATE          |                                                        |                                               |
| 19 | HEX DIGIT IN BUTTON2 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 20 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 21 | HEX DIGIT IN BUTTON2 PRESS-TIME (ms)|                                                        |                                               |
| 22 | HEX DIGIT IN                        |                                                        |                                               |
| 23 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 24 | DELIMITER                           |                                                        |                                               |
| 25 | HEX DIGIT IN BUTTON3 STATE          |                                                        |                                               |
| 26 | HEX DIGIT IN BUTTON3 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 27 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 28 | HEX DIGIT IN BUTTON3 PRESS-TIME (ms)|                                                        |                                               |
| 29 | HEX DIGIT IN                        |                                                        |                                               |
| 30 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 31 | DELIMITER                           |                                                        |                                               |
| 32 | HEX DIGIT IN BUTTON4 STATE          |                                                        |                                               |
| 33 | HEX DIGIT IN BUTTON4 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 34 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 35 | HEX DIGIT IN BUTTON4 PRESS-TIME (ms)|                                                        |                                               |
| 36 | HEX DIGIT IN                        |                                                        |                                               |
| 37 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 38 | DELIMITER                           |                                                        |                                               |
| 39 | HEX DIGIT IN BUTTON5 STATE          |                                                        |                                               |
| 40 | HEX DIGIT IN BUTTON5 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 41 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 42 | HEX DIGIT IN BUTTON5 PRESS-TIME (ms)|                                                        |                                               |
| 43 | HEX DIGIT IN                        |                                                        |                                               |
| 44 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 45 | DELIMITER                           |                                                        |                                               |
| 46 | HEX DIGIT IN BUTTON6 STATE          |                                                        |                                               |
| 47 | HEX DIGIT IN BUTTON6 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 48 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 49 | HEX DIGIT IN BUTTON6 PRESS-TIME (ms)|                                                        |                                               |
| 50 | HEX DIGIT IN                        |                                                        |                                               |
| 51 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 52 | DELIMITER                           |                                                        |                                               |
| 53 | HEX DIGIT IN BUTTON7 STATE          |                                                        |                                               |
| 54 | HEX DIGIT IN BUTTON7 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 55 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 56 | HEX DIGIT IN BUTTON7 PRESS-TIME (ms)|                                                        |                                               |
| 57 | HEX DIGIT IN                        |                                                        |                                               |
| 58 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 59 | DELIMITER                           |                                                        |                                               |
| 60 | HEX DIGIT IN BUTTON8 STATE          |                                                        |                                               |
| 61 | HEX DIGIT IN BUTTON8 ORDER 0 – 8    | ASCII CODING 0 - NOT PRESSED, 1…8 - PRESS ORDER        |                                               |
| 62 | HEX DIGIT IN MOST SIGNIFICANT BIT   |                                                        |                                               |
| 63 | HEX DIGIT IN BUTTON8 PRESS-TIME (ms)|                                                        |                                               |
| 64 | HEX DIGIT IN                        |                                                        |                                               |
| 65 | HEX DIGIT IN LEAST SIGNIFICANT BIT  |                                                        |                                               |
| 66 | <CR> 0x0D                           |                                                        |                                               |
| 67 | <LF> 0x0A                           |                                                        |                                               |
|    | TOTAL: 68 bytes                     |                                                        |                                               |
