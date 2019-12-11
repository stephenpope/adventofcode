using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC9
{
    public class Machine
    {
        private readonly int _sequenceCode;
        private readonly long[] _program;
        private long _output;
        private long _positionMemory;
        private long _relativeBase;
        private readonly bool _piped;

        public Machine(int sequenceCode, string rawData, bool piped = false)
        {
            _sequenceCode = sequenceCode;
            _program = rawData.Split(',').Select(long.Parse).ToArray();
            Array.Resize(ref _program, 2000);
            _piped = piped;
        }

        public (long outputValue, bool isComplete) Execute(int inputData)
        {
            _output = inputData;

            var inputStack = new Stack<int>();
            inputStack.Push(inputData);

            if (_positionMemory == 0 && _piped)
            {
                inputStack.Push(_sequenceCode);
            }

            for (var pos = _positionMemory; pos < _program.Length;)
            {
                //Process instruction (00000)
                var instruction = _program[pos].ToString("D5");

                //Get OpCode (xxx00)
                var opCode = int.Parse(instruction[3..]);

                long ReadArgument(int argPos)
                {
                    switch (instruction[3 - argPos])
                    {
                        case '1':
                            return _program[pos + argPos];
                        case '2':
                            return _program[_program[pos + argPos] + _relativeBase];
                        default:
                            return _program[_program[pos + argPos]];
                    }
                }

                void WriteArgument(int argPos, long input)
                {
                    if (instruction[3 - argPos] == '2')
                    {
                        _program[_program[pos + argPos] + _relativeBase] = input;
                    }
                    else
                    {
                        _program[_program[pos + argPos]] = input; 
                    }
                }

                switch (opCode)
                {
                    case 1: //ADD
                        WriteArgument(3, ReadArgument(1) + ReadArgument(2));

                        break;
                    case 2: //MULTIPLY
                        WriteArgument(3, ReadArgument(1) * ReadArgument(2));

                        break;
                    case 3: //INPUT
                        
                        var input = 0;

                        if (inputStack.Count > 0)
                        {
                            input = inputStack.Pop();
                        }

                        WriteArgument(1, input);

                        break;
                    case 4: //OUTPUT

                        _output = ReadArgument(1);

                        if (_output > 0 && _piped)
                        {
                            _positionMemory = pos + 2;

                            return (_output, false);
                        }

                        break;
                    case 5: //JUMP IF TRUE
                        
                        if (ReadArgument(1) > 0)
                        {
                            pos = ReadArgument(2);
                            continue;
                        }

                        break;
                    case 6: //JUMP IF FALSE

                        if (ReadArgument(1) == 0)
                        {
                            pos = ReadArgument(2);
                            continue;
                        }

                        break;
                    case 7: //LESS THAN

                        WriteArgument(3, ReadArgument(1) < ReadArgument(2) ? 1 : 0);

                        break;
                    case 8: //EQUALS
                        
                        WriteArgument(3, ReadArgument(1) == ReadArgument(2) ? 1 : 0);

                        break;
                    case 9: //RELATIVE BASE

                        _relativeBase += ReadArgument(1);
                        
                        break;
                    case 99:

                        return (_output, true);
                }

                pos += new[] {1, 4, 4, 2, 2, 3, 3, 4, 4, 2}[opCode];
            }

            return (_output, true);
        }
    }
}