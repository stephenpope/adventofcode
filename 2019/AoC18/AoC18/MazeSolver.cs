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
        private readonly (int x, int y)[] _directionLookup = {(0, -1), (-1, 0), (0, 1), (1, 0)};
        private readonly Dictionary<long, int> _distance = new Dictionary<long, int>(1923922);

        private long _keys;
        private long _startPoint;

        private int _height;
        private int _width;

        private char[] _mazeData;

        public static long SetNode(int x, int y, long keys)
        {
            return x | (y << 8) | (keys << 16);
        }

        public static long GetKeys(long node)
        {
            return (node >> 16) & 67108863;
        }
        
        public static long SetKey(long keys, int position)
        {
            return keys | 1 << position;
        }
        
        public static (int x, int y, long keys) GetNode(long node)
        {
            return ((int) (node & 255), (int) ((node >> 8) & 255), (node >> 16) & 67108863);
        }
        
        public static bool IsKeySet(long keys, int pos)
        {
            return (keys & (1 << pos)) != 0;
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
                    var currentChar = rowItem.Character;

                    if (rowItem.Character == '@')
                    {
                        _startPoint = SetNode(rowItem.X, line.Y, 0);
                        currentChar = '.';
                    }
                    
                    if(currentChar <= 122 && currentChar >= 97)
                    {
                        _keys = SetKey(_keys, currentChar - 'a');
                    }

                    _mazeData[rowItem.X + line.Y * _width] = currentChar;
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
                    var (x, y, currentKeys) = GetNode(current);

                    x += direction.x;
                    y += direction.y;

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
                
                //DrawMap(current);
            }

            return -1;
        }
        
        public void DrawMap(long current)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var tile = _mazeData[x + y * _width];
                    var currentPos = GetNode(current);

                    if (currentPos.x == x && currentPos.y == y)
                    {
                        tile = '@';
                    }

                    if (char.IsLower(tile))
                    {
                        if (IsKeySet(currentPos.keys, tile - 'a'))
                        {
                            tile = 'x';
                        }
                    }

                    if (char.IsUpper(tile))
                    {
                        if (IsKeySet(currentPos.keys, tile - 'A'))
                        {
                            tile = 'o';
                        }
                    }

                    Console.Write(tile);
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}