using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AoC13
{
    public class IntCodeMachine
    {
        private readonly long[] _memory;
        private long _position;
        private long _relativeBase;

        public Queue<long> InputQueue { get; set; }
        public Queue<long> OutputQueue { get; }
        
        public bool IsComplete { get; private set; }

        public IntCodeMachine(string program)
        {
            _position = 0;
            _relativeBase = 0;

            InputQueue = new Queue<long>();
            OutputQueue = new Queue<long>();

            _memory = program.Split(',').Select(long.Parse).ToArray();
            Array.Resize(ref _memory, 3000);
        }
        
        public ReturnCode Execute()
        {
            var returnCode = ReturnCode.Unknown;

            while (returnCode == ReturnCode.Unknown)
            {
                //Process instruction (00000)
                var instruction = _memory[_position].ToString("D5");

                //Get OpCode
                var opCode = int.Parse(instruction[3..]);

                long Parameter(int i)
                {
                    return instruction[3 - i] switch
                           {
                               '1' => (_position + i),
                               '2' => (_relativeBase + _memory[_position + i]),
                               _   => _memory[_position + i]
                           };
                }

                switch (opCode)
                {
                    case 1: // ADD
                        _memory[Parameter(3)] = _memory[Parameter(1)] + _memory[Parameter(2)];
                        break;
                    case 2: // MULTIPLY
                        _memory[Parameter(3)] = _memory[Parameter(1)] * _memory[Parameter(2)];
                        break;
                    case 3: // INPUT
                        if (InputQueue.Count == 0) { return ReturnCode.WaitingForInput; }
                        _memory[Parameter(1)] = InputQueue.Dequeue();
                        break;
                    case 4: // OUTPUT
                        OutputQueue.Enqueue(_memory[Parameter(1)]);
                        returnCode = ReturnCode.OutputWritten;
                        break;
                    case 5: // JUMP IF TRUE
                        if (_memory[Parameter(1)] != 0)
                        {
                            _position = _memory[Parameter(2)];
                            continue;
                        }

                        break;
                    case 6: //JUMP IF FALSE
                        if (_memory[Parameter(1)] == 0)
                        {
                            _position = _memory[Parameter(2)];
                            continue;
                        }

                        break;
                    case 7: //LESS THAN
                        if (_memory[Parameter(1)] < _memory[Parameter(2)])
                        {
                            _memory[Parameter(3)] = 1;
                        }
                        else
                        {
                            _memory[Parameter(3)] = 0;
                        }

                        break;
                    case 8: //EQUALS
                        if (_memory[Parameter(1)] == _memory[Parameter(2)])
                        {
                            _memory[Parameter(3)] = 1;
                        }
                        else
                        {
                            _memory[Parameter(3)] = 0;
                        }

                        break;
                    case 9: //RELATIVE BASE
                        _relativeBase += _memory[Parameter(1)];
                        break;
                    case 99:
                        returnCode = ReturnCode.Complete;
                        IsComplete = true;
                        continue;
                }

                _position += new[] {1, 4, 4, 2, 2, 3, 3, 4, 4, 2}[opCode];
            }
            
            return returnCode;
        }

        public void PatchMemory(int index, int value)
        {
            _memory[index] = value;
        }
    }
    
    public enum ReturnCode
    {
        Unknown = -1,
        WaitingForInput = 3,
        OutputWritten = 4,
        Complete = 99
    }
}