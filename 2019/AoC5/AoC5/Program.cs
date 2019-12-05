using System;
using System.Linq;

namespace AoC5
{
    class Program
    {
        static void Main(string[] args)
        {
            var dayFiveInput = "3,225,1,225,6,6,1100,1,238,225,104,0,1102,45,16,225,2,65,191,224,1001,224,-3172,224,4,224,102,8,223,223,1001,224,5,224,1,223,224,223,1102,90,55,225,101,77,143,224,101,-127,224,224,4,224,102,8,223,223,1001,224,7,224,1,223,224,223,1102,52,6,225,1101,65,90,225,1102,75,58,225,1102,53,17,224,1001,224,-901,224,4,224,1002,223,8,223,1001,224,3,224,1,224,223,223,1002,69,79,224,1001,224,-5135,224,4,224,1002,223,8,223,1001,224,5,224,1,224,223,223,102,48,40,224,1001,224,-2640,224,4,224,102,8,223,223,1001,224,1,224,1,224,223,223,1101,50,22,225,1001,218,29,224,101,-119,224,224,4,224,102,8,223,223,1001,224,2,224,1,223,224,223,1101,48,19,224,1001,224,-67,224,4,224,102,8,223,223,1001,224,6,224,1,223,224,223,1101,61,77,225,1,13,74,224,1001,224,-103,224,4,224,1002,223,8,223,101,3,224,224,1,224,223,223,1102,28,90,225,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,7,226,677,224,102,2,223,223,1005,224,329,1001,223,1,223,8,226,677,224,1002,223,2,223,1005,224,344,101,1,223,223,8,226,226,224,1002,223,2,223,1006,224,359,101,1,223,223,1008,677,226,224,1002,223,2,223,1005,224,374,1001,223,1,223,108,677,677,224,1002,223,2,223,1005,224,389,1001,223,1,223,1107,226,677,224,1002,223,2,223,1006,224,404,101,1,223,223,1008,226,226,224,102,2,223,223,1006,224,419,1001,223,1,223,7,677,226,224,1002,223,2,223,1005,224,434,101,1,223,223,1108,226,226,224,1002,223,2,223,1005,224,449,101,1,223,223,7,226,226,224,102,2,223,223,1005,224,464,101,1,223,223,108,677,226,224,102,2,223,223,1005,224,479,1001,223,1,223,1007,677,226,224,1002,223,2,223,1006,224,494,1001,223,1,223,1007,677,677,224,1002,223,2,223,1006,224,509,1001,223,1,223,107,677,677,224,1002,223,2,223,1005,224,524,101,1,223,223,1108,226,677,224,102,2,223,223,1006,224,539,1001,223,1,223,8,677,226,224,102,2,223,223,1005,224,554,101,1,223,223,1007,226,226,224,102,2,223,223,1006,224,569,1001,223,1,223,107,677,226,224,102,2,223,223,1005,224,584,1001,223,1,223,108,226,226,224,102,2,223,223,1006,224,599,1001,223,1,223,107,226,226,224,1002,223,2,223,1006,224,614,1001,223,1,223,1108,677,226,224,1002,223,2,223,1005,224,629,1001,223,1,223,1107,677,677,224,102,2,223,223,1005,224,644,1001,223,1,223,1008,677,677,224,102,2,223,223,1005,224,659,101,1,223,223,1107,677,226,224,1002,223,2,223,1006,224,674,101,1,223,223,4,223,99,226";
            var dayFiveProgram = dayFiveInput.Split(',').Select(int.Parse).ToArray();

            var dayFiveResultPart1 = ExecuteProgram(dayFiveProgram, 1, 0, true);
            Console.WriteLine($"DayFive Part One = {dayFiveResultPart1}");
            
            var dayFiveResultPart2 = ExecuteProgram(dayFiveProgram, 5, 0, true);
            Console.WriteLine($"DayFive Part Two = {dayFiveResultPart2}");
        }

        private static int ExecuteProgram(int[] inputProgram, int inputOne, int inputTwo, bool advanced = false)
        {
            var program = new int[inputProgram.Length];

            Array.Copy(inputProgram, program, inputProgram.Length);
            
            var cursor = 0;
            var finalOutput = 0;

            if (!advanced)
            {
                program[1] = inputOne; // Day 2 - Patch noun + verb
                program[2] = inputTwo;
            }

            for (var pos = 0; pos < program.Length; pos+=cursor)
            {
                //Process OPCode
                var instruction = program[pos];
                
                var opCode = instruction % 100;
                var paramOneIsImmediate = instruction / 100 % 10 == 1;
                var paramTwoIsImmediate = instruction / 1000 % 10 == 1;
                var paramThreeIsImmediate = instruction / 10000 % 10 == 1;
                
                switch (opCode)
                {
                    case 1: //ADD
                        
                        var addParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var addParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];

                        program[program[pos + 3]] = addParamOne + addParamTwo;

                        cursor = 4; // Move cursor by length of instruction.
                        
                        break;
                    case 2: //MULTIPLY
                        
                        var mulParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var mulParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];
                        
                        program[program[pos + 3]] = mulParamOne * mulParamTwo;

                        cursor = 4;
                        break;
                    case 3: //INPUT
                        
                        program[program[pos + 1]] = inputOne;
                        
                        cursor = 2;
                        break;
                    case 4: //OUTPUT
                        
                        var output = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];

                        if (output > 0)
                        {
                            finalOutput = output;
                        }

                        cursor = 2;
                        break;
                    case 5: //JUMP IF TRUE
                        
                        var jitParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var jitParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];

                        if (jitParamOne > 0)
                        {
                            pos = jitParamTwo;
                            cursor = 0; //No need to move the cursor (we just did it!)
                        }
                        else
                        {
                            cursor = 3; //Do nothing .. move to next instruction
                        }
                        break;
                    case 6: //JUMP IF FALSE
                        var jifParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var jifParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];

                        if (jifParamOne == 0)
                        {
                            pos = jifParamTwo;
                            cursor = 0; //No need to move the cursor (we just did it!)
                        }
                        else
                        {
                            cursor = 3; //Do nothing .. move to next instruction
                        }
                        
                        break;
                    case 7: //LESS THAN
                        
                        var ltParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var ltParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];
                        var ltParamThree = program[pos + 3];

                        if (ltParamOne < ltParamTwo)
                        {
                            program[ltParamThree] = 1;
                        }
                        else
                        {
                            program[ltParamThree] = 0;
                        }
                        
                        cursor = 4;
                        break;
                    case 8: //EQUALS
                        var eqParamOne = paramOneIsImmediate ? program[pos + 1] : program[program[pos + 1]];
                        var eqParamTwo = paramTwoIsImmediate ? program[pos + 2] : program[program[pos + 2]];
                        var eqParamThree = program[pos + 3];

                        if (eqParamOne == eqParamTwo)
                        {
                            program[eqParamThree] = 1;
                        }
                        else
                        {
                            program[eqParamThree] = 0;
                        }
                        
                        cursor = 4;
                        break;
                    case 99:
                        return advanced ? finalOutput : program[0];
                } 
            }

            return advanced ? finalOutput : program[0];
        }
    }
}