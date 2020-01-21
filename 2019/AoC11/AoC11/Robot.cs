using System;
using System.Collections.Generic;
using System.Drawing;

namespace AoC11
{
    public class Robot
    {
        private Point CurrentLocation = new Point(0, 0);
        private Direction CurrentDirection = Direction.Up;

        public readonly HashSet<Point> PaintedPanels = new HashSet<Point>();
        public readonly HashSet<Point> TotalPanels = new HashSet<Point>();
        private readonly IntCodeMachine _machine;

        public Robot(string program)
        {
            _machine = new IntCodeMachine(program);
        }

        public void Run()
        {
            while (!_machine.IsComplete)
            {
                StepRobot(StepMachine());
            }
        }

        private void StepRobot((int paintColour, int rotationType) input)
        {
            if (_machine.IsComplete)
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

        private void Paint(int paintColour)
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

        private bool IsCurrentPanelPainted()
        {
            return PaintedPanels.Contains(CurrentLocation);
        }

        private void Move()
        {
            CurrentLocation = CurrentDirection switch
                              {
                                  Direction.Up    => Point.Add(CurrentLocation, new Size(new Point(0, -1))),
                                  Direction.Left  => Point.Add(CurrentLocation, new Size(new Point(-1, 0))),
                                  Direction.Down  => Point.Add(CurrentLocation, new Size(new Point(0, 1))),
                                  Direction.Right => Point.Add(CurrentLocation, new Size(new Point(1, 0))),
                                  _ => throw new ArgumentOutOfRangeException()
                              };
        }

        private void TurnLeft()
        {
            CurrentDirection = CurrentDirection switch
                               {
                                   Direction.Up    => Direction.Left,
                                   Direction.Left  => Direction.Down,
                                   Direction.Down  => Direction.Right,
                                   Direction.Right => Direction.Up,
                                   _ => throw new ArgumentOutOfRangeException()
                               };
        }

        private void TurnRight()
        {
            CurrentDirection = CurrentDirection switch
                               {
                                   Direction.Up    => Direction.Right,
                                   Direction.Right => Direction.Down,
                                   Direction.Down  => Direction.Left,
                                   Direction.Left  => Direction.Up,
                                   _ => throw new ArgumentOutOfRangeException()
                               };
        }

        private (int paintColour, int rotationType) StepMachine()
        {
            var panelColour = IsCurrentPanelPainted() ? 1 : 0; // Check current panel colour
            var writeCount = 0;
            
            while (true)
            {
                var returnCode = _machine.Execute();
                
                if (returnCode == ReturnCode.Complete)
                {
                    return default;
                }

                if (returnCode == ReturnCode.WaitingForInput)
                {
                    _machine.InputQueue.Enqueue(panelColour);
                }
                
                if (returnCode == ReturnCode.OutputWritten)
                {
                    writeCount++;
                    
                    if (writeCount % 2 == 0)
                    {
                        break;
                    }
                }
            }

            var paintColour = _machine.OutputQueue.Dequeue();
            var rotation = _machine.OutputQueue.Dequeue();

            return ((int) paintColour, (int) rotation);
        }

        private enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }
    }
}