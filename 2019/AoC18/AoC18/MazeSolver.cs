using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC18
{
    public class MazeSolver
    {
        private readonly Point[] _directionLookup = {new Point(0, -1), new Point(-1, 0), new Point(0, 1), new Point(1, 0)};
        private readonly Dictionary<long, int> _distance = new Dictionary<long, int>();

        private long _keys;
        private long _startPoint;

        private int _height;
        private int _width;

        private char[] _mazeData;
        
        public static long SetNode(int x, int y, long keys)
        {
            return x | (y << 8) | (keys << 16);
        }
        
        public static long SetKey(long keys, int position)
        {
            return keys | 1 << position;
        }
        
        public static long GetKeys(long node)
        {
            return (node >> 16) & 67108863;
        }
        
        public static (int x, int y) GetPosition(long node)
        {
            return ((int) (node & 255), (int) ((node >> 8) & 255));
        }
        
        public static bool IsKeySet(long b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        public void LoadMaze(string input)
        {
            var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            _width = data[0].Trim().Length;
            _height = data.Length;
            _mazeData = new char[_width * _height];

            foreach (var line in data.Select((rowData, Y) => new {Y, rowData}))
            {
                foreach (var rowItem in line.rowData.Trim().Select((Character, X) => new {X, Character}))
                {
                    var index = rowItem.X + line.Y * _width;
                    var currentChar = rowItem.Character;

                    if (rowItem.Character == '@')
                    {
                        _startPoint = SetNode(rowItem.X, line.Y, 0);
                        currentChar = '.';
                    }

                    if (char.IsLower(currentChar))
                    {
                        _keys = SetKey(_keys, currentChar - 'a');
                    }

                    _mazeData[index] = currentChar;
                }
            }
        }

        public int WalkMap()
        {
            var queueCount = 0;
            var queueIndex = 0;

            var queue = new long[2000000];
            queue[queueCount++] = _startPoint;

            _distance[_startPoint] = 0;

            while (queueCount > queueIndex)
            {
                var current = queue[queueIndex++];

                if (GetKeys(current) == _keys)
                {
                    return _distance[current];
                }

                foreach (var direction in _directionLookup)
                {
                    var (x, y) = GetPosition(current);

                    x += direction.X;
                    y += direction.Y;
                    
                    var currentKeys = GetKeys(current);

                    var nextTile = _mazeData[x + y * _width];

                    if (nextTile == '#' || char.IsUpper(nextTile) && !IsKeySet(currentKeys, nextTile - 'A'))
                    {
                        continue;
                    }

                    if (char.IsLower(nextTile))
                    {
                        currentKeys = SetKey(currentKeys, nextTile - 'a');
                    }

                    var nextPosition = SetNode(x, y, currentKeys);

                    if (!_distance.ContainsKey(nextPosition))
                    {
                        _distance[nextPosition] = _distance[current] + 1;
                        queue[queueCount++] = nextPosition;
                    }
                }
            }

            return -1;
        }
    }
}