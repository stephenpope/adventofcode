using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC18
{
    public class AdvancedMazeSolver
    {
        private readonly Dictionary<Point, char> _mazeData = new Dictionary<Point, char>();

        private readonly bool[] _keys = new bool[26];

        private readonly AdvancedState _startPoint = new AdvancedState();

        private readonly Point[] _directionLookup = {new Point(0, -1), new Point(-1, 0), new Point(0, 1), new Point(1, 0)};

        private readonly DefaultDictionary<AdvancedState, int> _distance = new DefaultDictionary<AdvancedState, int>();

        private int _robots;

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
                        _startPoint.Position[_robots] = currentPoint;
                        currentChar = '.';
                        _robots++;
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
            var queue = new Queue<AdvancedState>();

            for (var i = 0; i < _robots; i++)
            {
                var start = new AdvancedState();
                Array.Copy(_startPoint.Position, start.Position, _startPoint.Position.Length);
                start.Active = i;
                
                _distance[start] = 0;
                queue.Enqueue(start);
            }

            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();

                if (currentPosition.Keys.Count(x => x) == _keys.Count(y => y))
                {
                    return _distance[currentPosition];
                }

                foreach (var direction in _directionLookup)
                {
                    // Make a copy
                    var nextPosition = currentPosition.Clone();
                    nextPosition.Position[currentPosition.Active] = currentPosition.Position[currentPosition.Active] + (Size) direction;
                    
                    var nextTile = _mazeData[nextPosition.Position[currentPosition.Active]];

                    if (nextTile == '#' || char.IsUpper(nextTile) && !currentPosition.Keys[nextTile - 'A'])
                    {
                        continue;
                    }

                    if (char.IsLower(nextTile))
                    {
                        nextPosition.Keys[nextTile - 'a'] = true;
                    }

                    for (var i = 0; i < _robots; i++)
                    {
                        if (i != currentPosition.Active && nextPosition.Keys.SequenceEqual(currentPosition.Keys))
                        {
                            continue;
                        }
                        
                        // I hate this !
                        var next = nextPosition.Clone();
                        next.Active = i; // Just for this line to work !
                        
                        if (!_distance.ContainsKey(next))
                        {
                            _distance[next] = _distance[currentPosition] + 1;
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            return -1;
        }
    }
}