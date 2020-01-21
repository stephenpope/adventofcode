using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC18
{
    public class MazeSolver
    {
        private readonly Dictionary<Point, char> _mazeData = new Dictionary<Point, char>();

        private readonly BitArray _keys = new BitArray(26);

        private State _startPoint;

        private readonly Point[] _directionLookup = {new Point(0, -1), new Point(-1, 0), new Point(0, 1), new Point(1, 0)};

        private readonly Dictionary<State, int> _distance = new Dictionary<State, int>();

        public void LoadMaze(string input)
        {
            var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in data.Select((rowData, Y) => new {Y, rowData}))
            {
                foreach (var rowItem in line.rowData.Trim().Select((Character, X) => new {X, Character}))
                {
                    var currentPoint = new Point(rowItem.X, line.Y);
                    var currentChar = rowItem.Character;

                    if (currentChar == '@')
                    {
                        _startPoint = new State(currentPoint, new BitArray(26));
                        currentChar = '.';
                    }

                    if (char.IsLower(currentChar))
                    {
                        _keys[currentChar - 'a'] = true;
                    }

                    _mazeData.Add(currentPoint, currentChar);
                }
            }
        }

        public int WalkMap()
        {
            var queue = new Queue<State>();
            queue.Enqueue(_startPoint);
            _distance.Add(_startPoint,0);

            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();

                if (currentPosition.Keys.BitEquals(_keys))
                {
                    return _distance[currentPosition];
                }

                foreach (var direction in _directionLookup)
                {
                    var nextPosition = new State(currentPosition.Position + (Size) direction, currentPosition.Keys);
                    var nextTile = _mazeData[nextPosition.Position];

                    if (nextTile == '#' || char.IsUpper(nextTile) && !currentPosition.Keys[nextTile - 'A'])
                    {
                        continue;
                    }

                    if (char.IsLower(nextTile))
                    {
                        nextPosition.Keys[nextTile - 'a'] = true;
                    }

                    if (!_distance.TryGetValue(nextPosition, out _))
                    {
                        _distance.Add(nextPosition, _distance[currentPosition] + 1);
                        queue.Enqueue(nextPosition);
                    }
                }
            }

            return -1;
        }
    }
}