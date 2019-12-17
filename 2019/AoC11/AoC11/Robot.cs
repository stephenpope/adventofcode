using System.Collections.Generic;
using System.Drawing;

namespace AoC11
{
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

        private void StepRobot((int paintColour, int rotationType) input)
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
                Direction.Up => Point.Add(CurrentLocation, new Size(new Point(0, -1))),
                Direction.Left => Point.Add(CurrentLocation, new Size(new Point(-1, 0))),
                Direction.Down => Point.Add(CurrentLocation, new Size(new Point(0, 1))),
                Direction.Right => Point.Add(CurrentLocation, new Size(new Point(1, 0)))
            };
        }

        private void TurnLeft()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Right,
                Direction.Right => Direction.Up
            };
        }

        private void TurnRight()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up
            };
        }

        private (int paintColour, int rotationType) StepMachine()
        {
            var panelColour = IsCurrentPanelPainted() ? 1 : 0; // Check current panel colour

            var stepOne = machine.Execute(panelColour);            // Run until output1 (paint colour)
            var stepTwo = machine.Execute(stepOne.outputValue);    // Run until output2 (direction)
            _isComplete = stepOne.isComplete || stepTwo.isComplete;

            return ((int) stepOne.outputValue, (int) stepTwo.outputValue);
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