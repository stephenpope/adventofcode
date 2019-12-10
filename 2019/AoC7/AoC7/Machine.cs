using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC7
{
    public class Machine
    {
        private readonly int _sequenceCode;
        private readonly int[] _program;
        private int _output;
        private int _positionMemory;

        public Machine(int sequenceCode, string rawData)
        {
            _sequenceCode = sequenceCode;
            _program = rawData.Split(',').Select(int.Parse).ToArray();
        }

        public (int outputValue, bool isComplete) Execute(int inputData)
        {
            _output = inputData;

            var inputStack = new Stack<int>();
            inputStack.Push(inputData);

            if (_positionMemory == 0)
            {
                inputStack.Push(_sequenceCode);
            }

            for (var pos = _positionMemory; pos < _program.Length;)
            {
                //Process instruction (00000)
                var instruction = _program[pos].ToString("D5");

                //Get OpCode (xxx00)
                var opCode = int.Parse(instruction[3..]);

                int GetArgument(int argPos)
                {
                    return instruction[3 - argPos] == '0'
                        ? _program[_program[pos + argPos]]
                        : _program[pos + argPos];
                }

                switch (opCode)
                {
                    case 1: //ADD

                        _program[_program[pos + 3]] = GetArgument(1) + GetArgument(2);

                        break;
                    case 2: //MULTIPLY

                        _program[_program[pos + 3]] = GetArgument(1) * GetArgument(2);

                        break;
                    case 3: //INPUT

                        var input = 0;

                        if (inputStack.Count > 0)
                        {
                            input = inputStack.Pop();
                        }

                        _program[_program[pos + 1]] = input;

                        break;
                    case 4: //OUTPUT

                        _output = GetArgument(1);

                        if (_output > 0)
                        {
                            _positionMemory = pos + 2;

                            return (_output, false);
                        }

                        break;
                    case 5: //JUMP IF TRUE

                        if (GetArgument(1) > 0)
                        {
                            pos = GetArgument(2);
                            continue;
                        }

                        break;
                    case 6: //JUMP IF FALSE

                        if (GetArgument(1) == 0)
                        {
                            pos = GetArgument(2);
                            continue;
                        }

                        break;
                    case 7: //LESS THAN

                        if (GetArgument(1) < GetArgument(2))
                        {
                            _program[_program[pos + 3]] = 1;
                        }
                        else
                        {
                            _program[_program[pos + 3]] = 0;
                        }

                        break;
                    case 8: //EQUALS

                        if (GetArgument(1) == GetArgument(2))
                        {
                            _program[_program[pos + 3]] = 1;
                        }
                        else
                        {
                            _program[_program[pos + 3]] = 0;
                        }

                        break;
                    case 99:

                        return (_output, true);
                }

                pos += new[] {1, 4, 4, 2, 2, 3, 3, 4, 4}[opCode];
            }

            return (_output, true);
        }
    }
}