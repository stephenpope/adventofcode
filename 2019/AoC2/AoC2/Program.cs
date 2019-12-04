using System;
using System.Linq;

namespace AoC2
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "1,0,0,0,99";
            //var input = "2,3,0,3,99";
            //var input = "2,4,4,5,99,0";
            //var input = "1,1,1,4,99,5,6,0,99";
            var input = "1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,6,1,19,1,5,19,23,1,13,23,27,1,6,27,31,2,31,13,35,1,9,35,39,2,39,13,43,1,43,10,47,1,47,13,51,2,13,51,55,1,55,9,59,1,59,5,63,1,6,63,67,1,13,67,71,2,71,10,75,1,6,75,79,1,79,10,83,1,5,83,87,2,10,87,91,1,6,91,95,1,9,95,99,1,99,9,103,2,103,10,107,1,5,107,111,1,9,111,115,2,13,115,119,1,119,10,123,1,123,10,127,2,127,10,131,1,5,131,135,1,10,135,139,1,139,2,143,1,6,143,0,99,2,14,0,0";
            var program = input.Split(',').Select(int.Parse).ToArray();
            
            Console.WriteLine("Part #1 Output: " + string.Join(",",ExecuteProgram(program, 12, 2)[0]));

            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    if (ExecuteProgram(program, noun, verb)[0] != 19690720) continue;
                    
                    var output = 100 * noun + verb;
                    
                    Console.WriteLine("Part #2 Output: " + output);
                    break;
                }
            }
            
        }

        private static int[] ExecuteProgram(int[] inputProgram, int noun, int verb)
        {
            var program = new int[inputProgram.Length];

            Array.Copy(inputProgram, program, inputProgram.Length);
            
            var cursor = 0;

            program[1] = noun;
            program[2] = verb;
            
            for (var pos = 0; pos < program.Length; pos+=cursor)
            {
                switch (program[pos])
                {
                    case 1: //ADD
                        program[program[pos + 3]] = program[program[pos + 1]] + program[program[pos + 2]];
                        cursor = 4;
                        break;
                    case 2: //MULTIPLY
                        program[program[pos + 3]] = program[program[pos + 1]] * program[program[pos + 2]];
                        cursor = 4;
                        break;
                    case 99:
                        return program;
                } 
            }

            return program;
        }
    }
}