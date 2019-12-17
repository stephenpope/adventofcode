﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC11
{
    class Program
    {
        static void Main(string[] args)
        {
            var robotOne = new Robot(new Machine(Data.rawData, true));
            robotOne.Run();
            Console.WriteLine("Part #1 - Total panels painted   : " + robotOne.TotalPanels.Count);
            Console.WriteLine("---------");
            
            var robotTwo = new Robot(new Machine(Data.rawData, true));
            robotTwo.PaintedPanels.Add(new Point(0, 0));
            robotTwo.Run();

            for (var y = robotTwo.TotalPanels.Min(x => x.Y); y <= robotTwo.TotalPanels.Max(x => x.Y); y++)
            {
                for (var x = robotTwo.TotalPanels.Min(x => x.X); x <= robotTwo.TotalPanels.Max(x => x.X); x++)
                {
                    Console.Write(robotTwo.PaintedPanels.Contains(new Point(x, y)) ? "#" : " ");
                }

                Console.WriteLine();
            }
        }
    }

    public class Robot
    {
        private readonly Machine machine;
        private bool _isComplete;

        private Point CurrentLocation = new Point(0, 0);
        private Direction CurrentDirection = Direction.Up;

        public readonly HashSet<Point> PaintedPanels = new HashSet<Point>();
        public readonly HashSet<Point> TotalPanels = new HashSet<Point>();

        public Robot(Machine brain)
        {
            machine = brain;
        }

        public void Run()
        {
            while (!_isComplete)
            {
                StepRobot(StepMachine());
            }
        }

        public void StepRobot((int paintColour, int rotationType) input)
        {
            if (_isComplete)
            {
                return;
            }

            Paint(input.paintColour);

            if (input.rotationType == 1)
            {
                TurnRight();
            }
            else
            {
                TurnLeft();
            }

            Move();
        }

        public void Paint(int paintColour)
        {
            TotalPanels.Add(CurrentLocation);

            if (paintColour == 1)
            {
                PaintedPanels.Add(CurrentLocation);
                return;
            }

            if (IsCurrentPanelPainted())
            {
                PaintedPanels.Remove(CurrentLocation);
            }
        }

        public bool IsCurrentPanelPainted()
        {
            return PaintedPanels.Contains(CurrentLocation);
        }

        public void Move()
        {
            CurrentLocation = CurrentDirection switch
            {
                Direction.Up => Point.Add(CurrentLocation, new Size(new Point(0, -1))),
                Direction.Left => Point.Add(CurrentLocation, new Size(new Point(-1, 0))),
                Direction.Down => Point.Add(CurrentLocation, new Size(new Point(0, 1))),
                Direction.Right => Point.Add(CurrentLocation, new Size(new Point(1, 0)))
            };
        }

        public void TurnLeft()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Right,
                Direction.Right => Direction.Up
            };
        }

        public void TurnRight()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up
            };
        }

        public (int paintColour, int rotationType) StepMachine()
        {
            var panelColour = IsCurrentPanelPainted() ? 1 : 0;

            var stepOne = machine.Execute(panelColour);
            var stepTwo = machine.Execute(stepOne.outputValue);
            _isComplete = stepOne.isComplete || stepTwo.isComplete;

            return ((int) stepOne.outputValue, (int) stepTwo.outputValue);
        }
        
        public enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }
    }

    public static class Data
    {
        public static string rawData = "3,8,1005,8,330,1106,0,11,0,0,0,104,1,104,0,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,1001,8,0,28,1,1103,17,10,1006,0,99,1006,0,91,1,102,7,10,3,8,1002,8,-1,10,101,1,10,10,4,10,108,1,8,10,4,10,1002,8,1,64,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,102,1,8,86,2,4,0,10,1006,0,62,2,1106,13,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,0,10,4,10,101,0,8,120,1,1109,1,10,1,105,5,10,3,8,102,-1,8,10,1001,10,1,10,4,10,108,1,8,10,4,10,1002,8,1,149,1,108,7,10,1006,0,40,1,6,0,10,2,8,9,10,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1002,8,1,187,1,1105,10,10,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1002,8,1,213,1006,0,65,1006,0,89,1,1003,14,10,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,102,1,8,244,2,1106,14,10,1006,0,13,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,1001,8,0,273,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,1,8,10,4,10,1001,8,0,295,1,104,4,10,2,108,20,10,1006,0,94,1006,0,9,101,1,9,9,1007,9,998,10,1005,10,15,99,109,652,104,0,104,1,21102,937268450196,1,1,21102,1,347,0,1106,0,451,21101,387512636308,0,1,21102,358,1,0,1105,1,451,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,21101,0,97751428099,1,21102,1,405,0,1105,1,451,21102,1,179355806811,1,21101,416,0,0,1106,0,451,3,10,104,0,104,0,3,10,104,0,104,0,21102,1,868389643008,1,21102,439,1,0,1105,1,451,21102,1,709475853160,1,21102,450,1,0,1105,1,451,99,109,2,22102,1,-1,1,21101,0,40,2,21101,482,0,3,21102,1,472,0,1105,1,515,109,-2,2106,0,0,0,1,0,0,1,109,2,3,10,204,-1,1001,477,478,493,4,0,1001,477,1,477,108,4,477,10,1006,10,509,1101,0,0,477,109,-2,2105,1,0,0,109,4,2101,0,-1,514,1207,-3,0,10,1006,10,532,21101,0,0,-3,21202,-3,1,1,22101,0,-2,2,21101,1,0,3,21101,0,551,0,1105,1,556,109,-4,2106,0,0,109,5,1207,-3,1,10,1006,10,579,2207,-4,-2,10,1006,10,579,22102,1,-4,-4,1105,1,647,21201,-4,0,1,21201,-3,-1,2,21202,-2,2,3,21101,0,598,0,1106,0,556,22101,0,1,-4,21102,1,1,-1,2207,-4,-2,10,1006,10,617,21101,0,0,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,639,22102,1,-1,1,21102,1,639,0,105,1,514,21202,-2,-1,-2,22201,-4,-2,-4,109,-5,2105,1,0";
    }
}