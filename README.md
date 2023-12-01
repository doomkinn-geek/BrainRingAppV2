# BrainRingAppV2
Вторая версия программы для регистрации нажатий кнопок на Брейн ринге. В отличие от первой версии программы, данная является своеобразным монитором. Все управление устройством осуществляется с помощью тач скрина, который присоединен к устройству.

# Протокол работы работы устройства следующий:

SPEC-BR PC COMMUNICATION PROTOCOL
19200, 8, N, 1 
Period: 1 packet every 200 ms


| N  | FORMAT                   | DESCRIPTION                   |
|----|--------------------------|-------------------------------|
| 0  | !                        | SOF                           |
| 1  | HEX DIGIT IN ASCII CODING| SYSTEM STATE (phases) :       |
|    |                          | - IDLE (RESET ALL)            |
|    |                          | - REGISTER FALSES             |
|    |                          | - REGISTER EARLY              |
|    |                          | - REGISTER OTHER              |
|    |                          | - STOPPED                     |
|    |                          | #define TFLAG_IDLE 0          |
|    |                          | #define TFLAG_FALSES 1        |
|    |                          | #define TFLAG_EARLY 2         |
|    |                          | #define TFLAG_NORMAL 3        |
|    |                          | #define TFLAG_STOPED 4        |
| 2  | HEX DIGIT IN ASCII CODING| RING current TIME (ms)        |
|    |                          | MOST SIGNIFICANT BIT          |
| 3  | HEX DIGIT IN ASCII CODING|                               |
| 4  | HEX DIGIT IN ASCII CODING|                               |
| 5  | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 6  | HEX DIGIT IN ASCII CODING| RING TOTAL TIME (ms)          |
|    |                          | MOST SIGNIFICANT BIT          |
| 7  | HEX DIGIT IN ASCII CODING|                               |
| 8  | HEX DIGIT IN ASCII CODING|                               |
| 9  | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 10 | :                        | DELIMITER                     |
| 11 | HEX DIGIT IN ASCII CODING| BUTTON1 STATE                 |
|    |                          | 0 - NOT PRESSED               |
|    |                          | 1 - FALSTART                  |
|    |                          | 2 - PRESSED @ EARLYTIME       |
|    |                          | 3 - PRESSED @ RINGTIME        |
|    |                          | #define BUTTON_STATE_IDLE 0   |
|    |                          | #define BUTTON_STATE_FSTART 1 |
|    |                          | #define BUTTON_STATE_EARLY 2  |
|    |                          | #define BUTTON_STATE_PRESSED 3|
| 12 | HEX DIGIT IN ASCII CODING| BUTTON1 ORDER                 |
|    |                          | 0 - NOT PRESSED               |
|    |                          | 1…8 - PRESS ORDER             |
| 13 | HEX DIGIT IN ASCII CODING| Button 1 PRESS-TIME (ms)      |
|    |                          | MOST SIGNIFICANT BIT          |
| 14 | HEX DIGIT IN ASCII CODING|                               |
| 15 | HEX DIGIT IN ASCII CODING|                               |
| 16 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 17 | :                        | DELIMITER                     |
| 18 | HEX DIGIT IN ASCII CODING| BUTTON2 STATE                 |
| 19 | HEX DIGIT IN ASCII CODING| BUTTON2 ORDER                 |
|    |                          | 0 – 8                         |
| 20 | HEX DIGIT IN ASCII CODING| BUTTON2 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 21 | HEX DIGIT IN ASCII CODING|                               |
| 22 | HEX DIGIT IN ASCII CODING|                               |
| 23 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 24 | :                        | DELIMITER                     |
| 25 | HEX DIGIT IN ASCII CODING| BUTTON3 STATE                 |
| 26 | HEX DIGIT IN ASCII CODING| BUTTON3 ORDER                 |
|    |                          | 0 – 8                         |
| 27 | HEX DIGIT IN ASCII CODING| BUTTON3 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 28 | HEX DIGIT IN ASCII CODING|                               |
| 29 | HEX DIGIT IN ASCII CODING|                               |
| 30 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 31 | :                        | DELIMITER                     |
| 32 | HEX DIGIT IN ASCII CODING| BUTTON4 STATE                 |
| 33 | HEX DIGIT IN ASCII CODING| BUTTON4 ORDER                 |
|    |                          | 0 – 8                         |
| 34 | HEX DIGIT IN ASCII CODING| BUTTON4 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 35 | HEX DIGIT IN ASCII CODING|                               |
| 36 | HEX DIGIT IN ASCII CODING|                               |
| 37 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 38 | :                        | DELIMITER                     |
| 39 | HEX DIGIT IN ASCII CODING| BUTTON5 STATE                 |
| 40 | HEX DIGIT IN ASCII CODING| BUTTON5 ORDER                 |
|    |                          | 0 – 8                         |
| 41 | HEX DIGIT IN ASCII CODING| BUTTON5 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 42 | HEX DIGIT IN ASCII CODING|                               |
| 43 | HEX DIGIT IN ASCII CODING|                               |
| 44 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 45 | :                        | DELIMITER                     |
| 46 | HEX DIGIT IN ASCII CODING| BUTTON6 STATE                 |
| 47 | HEX DIGIT IN ASCII CODING| BUTTON6 ORDER                 |
|    |                          | 0 – 8                         |
| 48 | HEX DIGIT IN ASCII CODING| BUTTON6 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 49 | HEX DIGIT IN ASCII CODING|                               |
| 50 | HEX DIGIT IN ASCII CODING|                               |
| 51 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 52 | :                        | DELIMITER                     |
| 53 | HEX DIGIT IN ASCII CODING| BUTTON7 STATE                 |
| 54 | HEX DIGIT IN ASCII CODING| BUTTON7 ORDER                 |
|    |                          | 0 – 8                         |
| 55 | HEX DIGIT IN ASCII CODING| BUTTON7 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 56 | HEX DIGIT IN ASCII CODING|                               |
| 57 | HEX DIGIT IN ASCII CODING|                               |
| 58 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 59 | :                        | DELIMITER                     |
| 60 | HEX DIGIT IN ASCII CODING| BUTTON8 STATE                 |
| 61 | HEX DIGIT IN ASCII CODING| BUTTON8 ORDER                 |
|    |                          | 0 – 8                         |
| 62 | HEX DIGIT IN ASCII CODING| BUTTON8 PRESS-TIME (ms)       |
|    |                          | MOST SIGNIFICANT BIT          |
| 63 | HEX DIGIT IN ASCII CODING|                               |
| 64 | HEX DIGIT IN ASCII CODING|                               |
| 65 | HEX DIGIT IN ASCII CODING| LEAST SIGNIFICANT BIT         |
| 66 | <CR>                     | 0x0D                          |
| 67 | <LF>                     | 0x0A                          |
|    | TOTAL: 68 bytes          |                               |
