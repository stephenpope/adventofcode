using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AoC18
{
    public class SuperMazeSolver
    {
        public static string testOne = @"
        ########################
        #@.....................#
        ######################.#
        #a.....................#
        ########################";

        private static int Width;
        private static int Height;
        private char[] _mazeData;
        private int Start_X;
        private int Start_Y;
        private int Goal_X;
        private int Goal_Y;
        private MazeNode[] _mapNodes;
        static Random rnd = new Random();
        //private Node[] _nodes = new Node[1000];

        private static HashSet<int> visitedHash = new HashSet<int>();
        //private static Queue<Node> queue = new Queue<Node>(1000);
        private static int[] _mapConnections;

        // var y = index/24;
        // var x = index - y - 23 * y;

        public void LoadMaze(string input)
        {
            var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Width = data[0].Trim().Length;
            Height = data.Length;

            _mazeData = new char[Width * Height];

            foreach (var line in data.Select((rowData, Y) => new {Y, rowData}))
            {
                foreach (var rowItem in line.rowData.Trim().Select((Character, X) => new {X, Character}))
                {
                    var index = rowItem.X + line.Y * Width;

                    //Console.WriteLine($"{rowItem.X},{line.Y} = {rowItem.Character} - Index: {index}");

                    if (rowItem.Character == '@')
                    {
                        Start_X = rowItem.X;
                        Start_Y = line.Y;
                    }

                    if (char.IsLower(rowItem.Character))
                    {
                        Goal_X = rowItem.X;
                        Goal_Y = line.Y;
                    }

                    _mazeData[index] = rowItem.Character;
                }
            }
        }

        public void ExploreMaze()
        {
            _mapNodes = new MazeNode[Width * Height];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _mapNodes[x + y * Width] = new MazeNode {x = x, y = y};
                }
            }

            var startNode = _mapNodes[Start_X + Start_Y * Width];
            startNode.parent = startNode;

            var exploreWatch = new Stopwatch();
            exploreWatch.Start();

            var lastNode = Explore(startNode);

            while (lastNode != startNode)
            {
                lastNode = Explore(lastNode);
            }

            exploreWatch.Stop();
            Console.WriteLine($"Explore: {exploreWatch.Elapsed.TotalMilliseconds} ms");
            
            Console.WriteLine($"Goal: {_mazeData[Goal_X + Goal_Y * Width]} (x:{Goal_X}, y:{Goal_Y})");

            var watch = new Stopwatch();
            watch.Start();
            
            var visitedArray = new int[Height];
            var nodes = new long[1000];
            var queueIndex = 0;
            var queueCount = 0;
            
            long current = 0;

            long SetNode(long x, long y, long distance, long connections, long parent)
            {
                return x | (y << 16) | (distance << 24) | (connections << 36) | (parent << 48);
            }

            (long x, long y, long distance, long connections, long parent) GetNode(long node)
            {
                return (node & 255, (node >> 16) & 255, (node >> 24) & 4095, (node >> 36) & 4095, (node >> 48) & 4095);
            }
            
            void SetVisited(int x, int y)
            {
                visitedArray[y] |= 1 << x;
            }
            
            bool IsAvailable(int x, int y)
            {
                return ((visitedArray[y] >> x) & 1) == 0;
            }
            
            void SetChildren(long current)
            {
                var x = (int)current & 255;
                var y = (int)(current >> 16) & 255;
                var distance = ((current >> 24) & 4095) + 1;
                var connections = (current >> 36) & 4095;
                var parentIndex = queueIndex - 1;

                if (y > 0 && (connections & 8) > 0 && IsAvailable(x, y - 1))
                {
                    var index = x + Width * (y - 1);
                    var nConnections = _mapConnections[index];
                    if ((nConnections & 2) > 0)
                    {
                        nodes[queueCount++] = SetNode(x, y - 1, distance, nConnections, parentIndex);
                        SetVisited(x, y - 1);
                    }
                }

        
                if (y < Height - 1 && (connections & 2) > 0 && IsAvailable(x, y + 1))
                {
                    int index = x + Width * (y + 1);
                    int nConnections = _mapConnections[index];
                    if ((nConnections & 8) > 0)
                    {
                        nodes[queueCount++] = SetNode(x, y + 1, distance, nConnections, parentIndex);
                        SetVisited(x, y + 1);
                    }
                }

                if (x > 0 && (connections & 1) > 0 && IsAvailable(x - 1, y))
                {
                    int index = x - 1 + Width * y;
                    int nConnections = _mapConnections[index];
                    if ((nConnections & 4) > 0)
                    {
                        nodes[queueCount++] = SetNode(x - 1, y, distance, nConnections, parentIndex);
                        SetVisited(x - 1, y);
                    }
                }

                if (x < Width - 1 && (connections & 4) > 0 && IsAvailable(x + 1, y))
                {
                    int index = x + 1 + Width * y;
                    int nConnections = _mapConnections[index];
                    if ((nConnections & 1) > 0)
                    {
                        nodes[queueCount++] = SetNode(x + 1, y, distance, nConnections, parentIndex);
                        SetVisited(x + 1, y);
                    }
                }
            }

            void Find()
            {
                var startIndex = Start_X + Start_Y * Width;
                var endCoordinates = Goal_X | Goal_Y << 16;
                var rootConnections = _mapConnections[startIndex];
                
                nodes[queueCount++] = SetNode(Start_X, Start_Y, 0, rootConnections, 0);
                SetVisited(Start_X, Start_Y);
                
                while (queueCount > queueIndex)
                {
                    current = nodes[queueIndex++];
                    
                    if ((current & 16777215) == endCoordinates)
                    {
                        return;    
                    }
                    
                    SetChildren(current);
                }
            }

            _mapConnections = new int[Width * Height];
            
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _mapConnections[j + i * Width] = _mapNodes[j + i * Width].connections;
                }
            }
            
            Find();
            
            // Console.WriteLine($"Height: {Height} - Width: {Width}");
            //

            //current = SetNode(100, 100, 1000, 1000, 1000);
            
            //Console.WriteLine(Convert.ToString(65535, 2));
            //
             // var endCoordinates = Goal_X | Goal_Y << 16;
             // current = SetNode(Goal_X, Goal_Y, 1000, 1000, 1000);
             // Console.WriteLine(Convert.ToString(current & 16777215, 2));
             // Console.WriteLine(Convert.ToString(endCoordinates, 2));
            
            var result = GetNode(current);
           
            Console.WriteLine($"{result.x},{result.y} - Dist: {result.distance} - Conn: {result.connections} - Parent: {result.parent}");

            Console.WriteLine("BFS Basic: " + watch.Elapsed.TotalMilliseconds + " milliseconds");
        }

        private MazeNode Explore(MazeNode node)
        {
            int x = 0;
            int y = 0;

            while (node.explored != 0)
            {
                //Randomly pick one direction
                var explore = 1 << rnd.Next(4);

                //If it has already been explored - try again
                if ((~node.explored & explore) > 0) continue;

                //Mark direction as explored
                node.explored &= ~explore;

                //Depending on chosen direction
                switch (explore)
                {
                    case 1: //Check if it's possible to go left
                        if (node.x - 1 >= 0 && _mazeData[(node.x - 1) + node.y * Width] != '#')
                        {
                            x = node.x - 1;
                            y = node.y;
                        }
                        else
                            continue;

                        break;

                    case 2: //Check if it's possible to go down
                        if (node.y + 1 < Height && _mazeData[node.x + (node.y + 1) * Width] != '#')
                        {
                            x = node.x;
                            y = node.y + 1;
                        }
                        else
                            continue;

                        break;


                    case 4: //Check if it's possible to go right	
                        if (node.x + 1 < Width && _mazeData[(node.x + 1) + node.y * Width] != '#')
                        {
                            x = node.x + 1;
                            y = node.y;
                        }
                        else
                            continue;

                        break;


                    case 8: //Check if it's possible to go up
                        if (node.y - 1 >= 0 && _mazeData[node.x + (node.y - 1) * Width] != '#')
                        {
                            x = node.x;
                            y = node.y - 1;
                        }
                        else
                            continue;

                        break;
                }

                var destination = _mapNodes[x + y * Width];
                if (destination.parent != null) continue;
                // if it already has a parent, skip it
                node.connections |= explore;
                destination.connections |= explore > 2 ? explore >> 2 : explore << 2;
                destination.parent = node;

                return destination;
            }

            return node.parent;
        }
    }

    public class MazeNode
    {
        public int x = 0;
        public int y = 0;
        public int explored = 15;
        public int connections = 0;
        public MazeNode parent = null;

        public override string ToString()
        {
            return $"{x},{y} - Explored: {Convert.ToString(explored, 2)} - Connections: {connections} - Parent: {parent?.x},{parent?.y}";
        }
    }
}