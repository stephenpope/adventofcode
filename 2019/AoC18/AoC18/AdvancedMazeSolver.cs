using System;
using System.Linq;

namespace AoC18
{
    public class AdvancedMazeSolver
    {
        private readonly StateDictionary _distance = new StateDictionary(6000000);

        private readonly (int x, int y)[] _directionLookup = {(0, -1), (-1, 0), (0, 1), (1, 0)};
        
        private static readonly ulong[] _mask = {0xFFFFFFFFFFFF0000, 0xFFFFFFFF0000FFFF, 0xFFFF0000FFFFFFFF, 0x0000FFFFFFFFFFFF };

        private ulong _startPoint;

        private long _keys;

        private int _robots;

        private int _height;

        private int _width;

        private char[] _mazeData;

        public static uint SetPosition(uint x, uint y)
        {
            return x | (y << 8);
        }

        public static ulong LoadPosition(ulong source, uint position, int offset)
        {
            source &= _mask[offset];
            return source | (ulong)position << (offset * 16);
        }
        
        public uint UnloadPosition(ulong source, int offset)
        {
            return (uint) (source >> (16 * offset) & 65535);
        }

        public static (int x, int y) GetPosition(uint node)
        {
            return ((int x, int y)) (node & 255, (node >> 8) & 255);
        }

        public static long SetKey(long keys, int position)
        {
            return keys | 1 << position;
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
                        _startPoint = LoadPosition(_startPoint, SetPosition((uint)rowItem.X, (uint)line.Y), _robots);
                        currentChar = '.';
                        _robots++;
                    }

                    if (currentChar <= 122 && currentChar >= 97)
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

            var queue = new State[8000000];
        
            for (var i = 0; i < _robots; i++)
            {
                var start = new State { Position = _startPoint, Keys = 0, Active = i };
                _distance[start] = 0;
                queue[queueCount++] = start;
            }
        
            while (queueCount > queueIndex)
            {
                var current = queue[queueIndex++];
                
                if (current.Keys == _keys)
                {
                    return _distance[current];
                }
                
                foreach (var direction in _directionLookup)
                {
                    var (x, y) = GetPosition(UnloadPosition(current.Position, current.Active));
                    
                    x += direction.x;
                    y += direction.y;

                    var nextPosition = LoadPosition(current.Position, SetPosition((uint)x, (uint)y), current.Active);
                    var nextKeys = current.Keys;
                    var nextTile = _mazeData[x + y * _width];
                
                    if (nextTile == '#' || nextTile <= 90 && nextTile >= 65 && !IsKeySet(nextKeys, nextTile - 'A'))
                    {
                        continue;
                    }
                
                    if (nextTile <= 122 && nextTile >= 97)
                    {
                        nextKeys = SetKey(nextKeys, nextTile - 'a');
                    }
                
                    for (var i = 0; i < _robots; i++)
                    {
                        if (i != current.Active && nextKeys == current.Keys)
                        {
                            continue;
                        }
                        
                        var next = new State { Position = nextPosition, Keys = nextKeys, Active = i};
                
                        if (!_distance.ContainsKey(next))
                        {
                            _distance[next] = _distance[current] + 1;
                            queue[queueCount++] = next;
                        }
                    }
                }
            }
        
            return -1;
        }
    }
}