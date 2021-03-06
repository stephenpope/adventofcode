﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC17
{
    class Program
    {
        static void Main(string[] args)
        {
            var ascMachine = new AsciiMachine(Data.rawData);
            ascMachine.Run();
            //ascMachine.Draw();

            Console.WriteLine("Part I - Total : " + ascMachine.CalculateTotal());
            Console.WriteLine("-----");

            var directions = ascMachine.CalculatePath();
            var (program, a, b, c) = ascMachine.Compress(directions);

            // Console.WriteLine("Path found : " + directions);
            // Console.WriteLine($"Main Program : {program}");
            // Console.WriteLine($"Pattern A : {a}");
            // Console.WriteLine($"Pattern B : {b}");
            // Console.WriteLine($"Pattern C : {c}");
            // Console.WriteLine("-----");

            var machineTwo = new IntCodeMachine(Data.rawData);
            machineTwo.PatchMemory(0, 2); // Set correct mode.

            var inputArray = string.Join('\n', program, a, b, c, "n", string.Empty)
                                   .ToCharArray();

            foreach (var input in inputArray)
            {
                machineTwo.InputQueue.Enqueue(input);
            }

            long result = 0;
            
            while (true)
            {
                var returnCode = machineTwo.Execute();

                if (returnCode == ReturnCode.Complete)
                {
                    break;
                }

                if (returnCode == ReturnCode.OutputWritten)
                {
                    result =  machineTwo.OutputQueue.Dequeue();
                }
            }

            Console.WriteLine("Part II - Space Dust: " + result);
        }
    }

    public class AsciiMachine
    {
        private readonly IntCodeMachine _machine;

        //private bool _isComplete;
        private readonly Dictionary<Point, char> _scaffoldData;

        private readonly Dictionary<char, int> positionLookup = new Dictionary<char, int>
                                                                {{'^', 0}, {'<', 1}, {'v', 2}, {'>', 3}};

        private readonly Point[] directionLookup =
            {new Point(0, -1), new Point(-1, 0), new Point(0, 1), new Point(1, 0)};

        private Point robotPosition;
        private int robotDirection;

        public AsciiMachine(string program)
        {
            _machine = new IntCodeMachine(program);
            _scaffoldData = new Dictionary<Point, char>();
        }

        public void Run()
        {
            var currentPosition = new Point();

            while (!_machine.IsComplete)
            {
                var result = (char) StepMachine(1);

                if (_machine.IsComplete)
                {
                    break;
                }

                switch (result)
                {
                    case '\n':
                        currentPosition.X = 0;
                        currentPosition.Y++;
                        break;
                    case '^':
                    case '<':
                    case 'v':
                    case '>':
                    {
                        robotPosition = currentPosition;
                        robotDirection = positionLookup[result];
                        goto default;
                    }
                    default:
                    {
                        _scaffoldData[currentPosition] = result;
                        currentPosition.X++;
                        break;
                    }
                }
            }
        }

        public int CalculateTotal()
        {
            var sum = 0;

            foreach (var (key, val) in _scaffoldData)
            {
                if (val == '#'
                    && _scaffoldData.GetValueOrDefault(key + (Size) directionLookup[0], '.') == '#'
                    && _scaffoldData.GetValueOrDefault(key + (Size) directionLookup[1], '.') == '#'
                    && _scaffoldData.GetValueOrDefault(key + (Size) directionLookup[2], '.') == '#'
                    && _scaffoldData.GetValueOrDefault(key + (Size) directionLookup[3], '.') == '#')
                {
                    sum += key.X * key.Y;
                }
            }

            return sum;
        }

        public string CalculatePath()
        {
            var path = string.Empty;

            while (true)
            {
                // Try to turn 90 degrees until we find a valid path ..
                if (_scaffoldData.GetValueOrDefault(robotPosition + (Size) directionLookup[(robotDirection + 1) % 4]) == '#')
                {
                    robotDirection = (robotDirection + 1) % 4;
                    path += "L,";
                }
                else if (_scaffoldData.GetValueOrDefault(robotPosition + (Size) directionLookup[(robotDirection + 3) % 4]) == '#')
                {
                    robotDirection = (robotDirection + 3) % 4;
                    path += "R,";
                }
                else
                {
                    break;
                }

                // Go forwards until we cant go any further, then we stop and turn - no need to check on each move (avoids issues with intersections)
                var dist = 0;

                while (_scaffoldData.GetValueOrDefault(robotPosition + (Size) directionLookup[robotDirection]) == '#')
                {
                    robotPosition += (Size) directionLookup[robotDirection];
                    dist++;
                }

                path += dist + ",";
            }

            return path;
        }

        public (string program, string a, string b, string c) Compress(string path)
        {
            var a = string.Empty;
            var b = string.Empty;
            var c = string.Empty;

            //Find 3 repeating patterns in a string
            void Reduce()
            {
                for (var i = 2; i <= 21; i++)
                {
                    for (var j = 2; j <= 21; j++)
                    {
                        for (var k = 2; k <= 21; k++)
                        {
                            var str = path;

                            a = str[new Range(0, i)];
                            str = str.Replace(a, string.Empty);

                            b = str[new Range(0, j)];
                            str = str.Replace(b, string.Empty);

                            c = str[new Range(0, k)];
                            str = str.Replace(c, string.Empty);

                            if (str == string.Empty)
                            {
                                return; //When string is blank we have found all 3, so exit!
                            }
                        }
                    }
                }
            }

            Reduce();

            a = a.Trim(',');
            b = b.Trim(',');
            c = c.Trim(',');

            path = path.Replace(a, "A");
            path = path.Replace(b, "B");
            path = path.Replace(c, "C");

            path = path.Trim(',');

            return (path, a, b, c);
        }

        public void Draw()
        {
            var maxX = _scaffoldData.Keys.Max(x => x.X);
            var maxY = _scaffoldData.Keys.Max(y => y.Y);

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    Console.Write(_scaffoldData[new Point(x, y)]);
                }

                Console.WriteLine();
            }
        }

        private long StepMachine(long input)
        {
            while (true)
            {
                var returnCode = _machine.Execute();

                if (returnCode == ReturnCode.Complete)
                {
                    return default;
                }

                if (returnCode == ReturnCode.WaitingForInput)
                {
                    _machine.InputQueue.Enqueue(input);
                }

                if (returnCode == ReturnCode.OutputWritten)
                {
                    return _machine.OutputQueue.Dequeue();
                }
            }
        }
    }

    public static class Data
    {
        public static string rawData =
            "1,330,331,332,109,3492,1101,1182,0,15,1101,1483,0,24,1002,0,1,570,1006,570,36,101,0,571,0,1001,570,-1,570,1001,24,1,24,1105,1,18,1008,571,0,571,1001,15,1,15,1008,15,1483,570,1006,570,14,21101,58,0,0,1105,1,786,1006,332,62,99,21101,333,0,1,21101,73,0,0,1105,1,579,1102,1,0,572,1101,0,0,573,3,574,101,1,573,573,1007,574,65,570,1005,570,151,107,67,574,570,1005,570,151,1001,574,-64,574,1002,574,-1,574,1001,572,1,572,1007,572,11,570,1006,570,165,101,1182,572,127,1001,574,0,0,3,574,101,1,573,573,1008,574,10,570,1005,570,189,1008,574,44,570,1006,570,158,1105,1,81,21101,340,0,1,1106,0,177,21102,477,1,1,1106,0,177,21102,514,1,1,21101,176,0,0,1106,0,579,99,21102,184,1,0,1106,0,579,4,574,104,10,99,1007,573,22,570,1006,570,165,1002,572,1,1182,21102,1,375,1,21102,211,1,0,1105,1,579,21101,1182,11,1,21102,222,1,0,1105,1,979,21102,1,388,1,21101,233,0,0,1105,1,579,21101,1182,22,1,21101,244,0,0,1105,1,979,21102,401,1,1,21102,1,255,0,1105,1,579,21101,1182,33,1,21101,266,0,0,1106,0,979,21101,414,0,1,21102,1,277,0,1105,1,579,3,575,1008,575,89,570,1008,575,121,575,1,575,570,575,3,574,1008,574,10,570,1006,570,291,104,10,21102,1,1182,1,21101,0,313,0,1106,0,622,1005,575,327,1101,0,1,575,21101,327,0,0,1106,0,786,4,438,99,0,1,1,6,77,97,105,110,58,10,33,10,69,120,112,101,99,116,101,100,32,102,117,110,99,116,105,111,110,32,110,97,109,101,32,98,117,116,32,103,111,116,58,32,0,12,70,117,110,99,116,105,111,110,32,65,58,10,12,70,117,110,99,116,105,111,110,32,66,58,10,12,70,117,110,99,116,105,111,110,32,67,58,10,23,67,111,110,116,105,110,117,111,117,115,32,118,105,100,101,111,32,102,101,101,100,63,10,0,37,10,69,120,112,101,99,116,101,100,32,82,44,32,76,44,32,111,114,32,100,105,115,116,97,110,99,101,32,98,117,116,32,103,111,116,58,32,36,10,69,120,112,101,99,116,101,100,32,99,111,109,109,97,32,111,114,32,110,101,119,108,105,110,101,32,98,117,116,32,103,111,116,58,32,43,10,68,101,102,105,110,105,116,105,111,110,115,32,109,97,121,32,98,101,32,97,116,32,109,111,115,116,32,50,48,32,99,104,97,114,97,99,116,101,114,115,33,10,94,62,118,60,0,1,0,-1,-1,0,1,0,0,0,0,0,0,1,24,22,0,109,4,1202,-3,1,587,20101,0,0,-1,22101,1,-3,-3,21101,0,0,-2,2208,-2,-1,570,1005,570,617,2201,-3,-2,609,4,0,21201,-2,1,-2,1106,0,597,109,-4,2105,1,0,109,5,2102,1,-4,630,20102,1,0,-2,22101,1,-4,-4,21102,1,0,-3,2208,-3,-2,570,1005,570,781,2201,-4,-3,652,21001,0,0,-1,1208,-1,-4,570,1005,570,709,1208,-1,-5,570,1005,570,734,1207,-1,0,570,1005,570,759,1206,-1,774,1001,578,562,684,1,0,576,576,1001,578,566,692,1,0,577,577,21102,702,1,0,1106,0,786,21201,-1,-1,-1,1105,1,676,1001,578,1,578,1008,578,4,570,1006,570,724,1001,578,-4,578,21102,731,1,0,1106,0,786,1106,0,774,1001,578,-1,578,1008,578,-1,570,1006,570,749,1001,578,4,578,21101,756,0,0,1105,1,786,1105,1,774,21202,-1,-11,1,22101,1182,1,1,21102,774,1,0,1105,1,622,21201,-3,1,-3,1105,1,640,109,-5,2106,0,0,109,7,1005,575,802,20101,0,576,-6,20101,0,577,-5,1106,0,814,21101,0,0,-1,21102,1,0,-5,21102,0,1,-6,20208,-6,576,-2,208,-5,577,570,22002,570,-2,-2,21202,-5,49,-3,22201,-6,-3,-3,22101,1483,-3,-3,2101,0,-3,843,1005,0,863,21202,-2,42,-4,22101,46,-4,-4,1206,-2,924,21101,0,1,-1,1105,1,924,1205,-2,873,21101,35,0,-4,1105,1,924,1202,-3,1,878,1008,0,1,570,1006,570,916,1001,374,1,374,2102,1,-3,895,1102,1,2,0,2102,1,-3,902,1001,438,0,438,2202,-6,-5,570,1,570,374,570,1,570,438,438,1001,578,558,921,21002,0,1,-4,1006,575,959,204,-4,22101,1,-6,-6,1208,-6,49,570,1006,570,814,104,10,22101,1,-5,-5,1208,-5,41,570,1006,570,810,104,10,1206,-1,974,99,1206,-1,974,1102,1,1,575,21102,1,973,0,1106,0,786,99,109,-7,2106,0,0,109,6,21102,0,1,-4,21102,1,0,-3,203,-2,22101,1,-3,-3,21208,-2,82,-1,1205,-1,1030,21208,-2,76,-1,1205,-1,1037,21207,-2,48,-1,1205,-1,1124,22107,57,-2,-1,1205,-1,1124,21201,-2,-48,-2,1105,1,1041,21102,1,-4,-2,1105,1,1041,21102,1,-5,-2,21201,-4,1,-4,21207,-4,11,-1,1206,-1,1138,2201,-5,-4,1059,1201,-2,0,0,203,-2,22101,1,-3,-3,21207,-2,48,-1,1205,-1,1107,22107,57,-2,-1,1205,-1,1107,21201,-2,-48,-2,2201,-5,-4,1090,20102,10,0,-1,22201,-2,-1,-2,2201,-5,-4,1103,1202,-2,1,0,1106,0,1060,21208,-2,10,-1,1205,-1,1162,21208,-2,44,-1,1206,-1,1131,1105,1,989,21101,0,439,1,1106,0,1150,21101,0,477,1,1105,1,1150,21101,514,0,1,21102,1,1149,0,1106,0,579,99,21101,0,1157,0,1105,1,579,204,-2,104,10,99,21207,-3,22,-1,1206,-1,1138,2101,0,-5,1176,2102,1,-4,0,109,-6,2105,1,0,40,9,40,1,7,1,40,1,7,1,40,1,7,1,22,9,9,1,7,1,22,1,7,1,9,1,7,1,22,1,7,1,9,1,7,1,22,1,7,1,9,1,7,1,22,1,7,1,9,1,7,1,22,1,7,1,9,1,7,1,10,11,1,1,7,1,7,11,10,1,9,1,1,1,7,1,7,1,1,1,18,1,9,1,1,1,7,11,18,1,9,1,1,1,15,1,20,1,3,9,15,1,20,1,3,1,5,1,17,1,20,1,3,1,5,1,17,1,20,1,3,1,5,1,17,1,20,1,3,1,5,1,17,1,20,1,3,1,5,1,17,1,18,11,1,13,5,1,18,1,1,1,3,1,3,1,13,1,5,1,10,11,3,11,3,11,10,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,1,9,1,3,1,16,1,7,1,9,13,1,11,6,1,7,1,19,1,1,1,11,1,6,9,19,13,1,1,36,1,9,1,1,1,36,1,9,1,1,1,36,1,9,1,1,1,36,1,9,1,1,1,36,1,9,1,1,1,36,1,3,9,36,1,9,1,38,11,8";
    }
}