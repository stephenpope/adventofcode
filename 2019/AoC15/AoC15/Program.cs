﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC15
{
    class Program
    {
        static void Main(string[] args)
        {
            var robot = new RepairRobot(new Machine(Data.RawData, true));
            robot.Run();
        }
    }

    public class RepairRobot
    {
        private readonly Machine _machine;
        
        private readonly Dictionary<Point, long> _mazeData;
        private Point _endLocation;
        private Point _startLocation;
        private int sizeX;
        private int sizeY;
        private int offsetX;
        private int offsetY;

        public RepairRobot(Machine machine)
        {
            _machine = machine;
            _mazeData = new Dictionary<Point, long>();
            _startLocation = new Point(0, 0);
            _mazeData.Add(_startLocation, 1);
        }

        public void Run()
        {
            //Walk the whole maze first (cache the data).
            Walk(_startLocation);

            var minX = _mazeData.Keys.Min(k => k.X);
            var maxX = _mazeData.Keys.Max(k => k.X);
            var minY = _mazeData.Keys.Min(k => k.Y);
            var maxY = _mazeData.Keys.Max(k => k.Y);
            
            //DrawMaze(minX, maxX, minY, maxY);
            
            offsetX = Math.Abs(minX); // Convert minus numbers to absolute to create offset
            offsetY = Math.Abs(minY);
            
            sizeX = maxX + offsetX; // Use offset to correct minus location values
            sizeY = maxY + offsetY;
            
            var startPoint = new Point(_startLocation.X + offsetX, _startLocation.Y + offsetY);
            var endPoint = new Point(_endLocation.X + offsetX, _endLocation.Y + offsetY);
            
            var distance = BFS(startPoint, endPoint);
            Console.WriteLine("Part I  - Shortest Path   : " + distance);
            
            var fullDistance = BFS(endPoint, endPoint, true);
            Console.WriteLine("Part II - Minutes to fill : " + fullDistance);

        }

        public void Walk(Point location)
        {
            int[] xNum = {0, 0, -1, 1};
            int[] yNum = {-1, 1, 0, 0};
            long[] dirNum = {1, 2, 3, 4}; //north (1), south (2), west (3), and east (4)
            long[] oppNum = {2, 1, 4, 3}; //south (2), north (1), east (4), and west (3)

            //Look in 4 directions ..
            for (var i = 0; i < 4; i++)
            {
                var lookPoint = new Point(location.X + xNum[i], location.Y + yNum[i]);
        
                if (_mazeData.ContainsKey(lookPoint)) continue;
        
                // Try to step in that direction ..
                var result = StepMachine(dirNum[i]);
                _mazeData.Add(lookPoint, result);
                
                if (result == 0) continue; // Wall

                Walk(lookPoint);
                
                //Step back to where you were..
                StepMachine(oppNum[i]);

                if (result == 2) // OxygenPump
                {
                    _endLocation = new Point(lookPoint.X, lookPoint.Y);
                }
            }
        }

        public long StepMachine(long input)
        {
            return _machine.Execute(input).outputValue;
        }

        public long BFS(Point startNode, Point targetNode, bool fullWalk = false)
        {
            int[] rowNum = {-1, 0, 0, 1};
            int[] colNum = {0, -1, 1, 0};
        
            var visited = new bool[sizeX, sizeY]; //Row + Col
            var longestDistance = 0;
            
            // Mark the startNode as visited 
            visited[startNode.X, startNode.Y] = true;

            // Create a queue for BFS 
            var q = new Queue<QueueNode>();
        
            // Distance of startNode is 0 
            var s = new QueueNode(startNode, 0);
            q.Enqueue(s); // Enqueue source cell 
        
            // Do a BFS starting from startNode 
            while (q.Count != 0)
            {
                var currentNode = q.Peek();
                var point = currentNode.Point;
        
                // If we have reached the targetNode, we are done 
                if (!fullWalk && point.X == targetNode.X && point.Y == targetNode.Y)
                {
                    return currentNode.Distance;
                }
        
                // Otherwise dequeue the first node in the queue and enqueue its adjacent neighbours 
                q.Dequeue();
        
                // Look in 4 directions
                for (var i = 0; i < 4; i++)
                {
                    var row = point.X + rowNum[i];
                    var col = point.Y + colNum[i];
        
                    // if adjacent neighbour is valid, has path and not visited yet, is not a Wall (0), then enqueue it. 
                    if (row < 0 || row >= sizeX || col < 0 || col >= sizeY || _mazeData[new Point(row - offsetX, col - offsetY)] == 0 || visited[row, col])
                    {
                        continue;
                    }
                    
                    // Save distance to furthest point (Part II)
                    if (currentNode.Distance + 1 > longestDistance)
                    {
                        longestDistance = currentNode.Distance + 1;
                    }
                    
                    // mark node as visited and enqueue it 
                    visited[row, col] = true;
                    q.Enqueue(new QueueNode(new Point(row, col), currentNode.Distance + 1));
                }
            }
            
            // Maze cannot be solved (but return longest distance for part II)
            return longestDistance;
        }

        public void DrawMaze(int minX, int maxX, int minY, int maxY)
        {
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var point = new Point(x, y);
                    if (_mazeData.ContainsKey(point))
                    {
                        switch (_mazeData[point])
                        {
                            case 0:
                                Console.Write("###");
                                break;
                            case 1:
                                Console.Write(point == _startLocation ? " S " : " . ");
                                break;
                            case 2:
                                Console.Write(" O ");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        Console.Write("###");
                    }
                }
        
                Console.WriteLine();
            }
        }
    }

    public class QueueNode
    {
        public Point Point;

        public readonly int Distance;

        public QueueNode(Point pt, int dist)
        {
            Point = pt;
            Distance = dist;
        }
    }

    public static class Data
    {
        public const string RawData =
            "3,1033,1008,1033,1,1032,1005,1032,31,1008,1033,2,1032,1005,1032,58,1008,1033,3,1032,1005,1032,81,1008,1033,4,1032,1005,1032,104,99,102,1,1034,1039,1002,1036,1,1041,1001,1035,-1,1040,1008,1038,0,1043,102,-1,1043,1032,1,1037,1032,1042,1105,1,124,102,1,1034,1039,101,0,1036,1041,1001,1035,1,1040,1008,1038,0,1043,1,1037,1038,1042,1105,1,124,1001,1034,-1,1039,1008,1036,0,1041,101,0,1035,1040,1001,1038,0,1043,101,0,1037,1042,1105,1,124,1001,1034,1,1039,1008,1036,0,1041,101,0,1035,1040,101,0,1038,1043,101,0,1037,1042,1006,1039,217,1006,1040,217,1008,1039,40,1032,1005,1032,217,1008,1040,40,1032,1005,1032,217,1008,1039,5,1032,1006,1032,165,1008,1040,9,1032,1006,1032,165,1102,1,2,1044,1105,1,224,2,1041,1043,1032,1006,1032,179,1102,1,1,1044,1105,1,224,1,1041,1043,1032,1006,1032,217,1,1042,1043,1032,1001,1032,-1,1032,1002,1032,39,1032,1,1032,1039,1032,101,-1,1032,1032,101,252,1032,211,1007,0,73,1044,1106,0,224,1101,0,0,1044,1106,0,224,1006,1044,247,101,0,1039,1034,1002,1040,1,1035,1002,1041,1,1036,1002,1043,1,1038,101,0,1042,1037,4,1044,1105,1,0,43,57,94,36,95,30,10,40,88,72,99,97,53,21,87,48,77,40,75,69,46,98,78,22,21,38,17,12,96,34,94,81,18,49,92,1,26,67,48,15,80,51,60,92,9,77,89,64,15,85,53,94,84,99,70,7,8,69,79,79,41,62,98,22,94,92,69,97,65,96,47,99,71,4,75,10,89,85,13,89,93,93,33,46,80,61,80,75,47,99,54,63,54,57,99,80,97,77,48,33,97,95,92,20,75,3,90,84,1,50,15,94,80,95,93,70,22,3,74,69,27,99,91,66,99,1,67,12,94,31,78,83,51,97,25,4,92,85,3,96,60,5,98,69,23,95,70,92,99,1,5,84,51,87,60,67,56,98,44,80,71,81,59,58,97,82,48,87,4,76,87,45,23,75,62,89,29,37,83,22,89,81,48,64,92,30,13,90,89,83,50,49,14,89,2,34,39,84,88,21,1,81,41,74,95,89,37,82,30,87,11,93,78,67,99,8,95,84,26,93,9,95,7,18,93,94,55,96,50,92,97,43,88,53,22,91,91,35,5,79,34,66,56,24,95,49,86,72,98,52,19,81,10,90,78,12,76,8,37,87,62,80,98,52,19,40,97,83,70,18,94,77,62,87,13,35,90,35,78,68,84,89,77,13,71,19,81,54,96,88,22,40,99,24,62,85,37,95,97,89,64,30,18,98,95,9,27,76,85,49,99,31,55,71,89,95,86,94,69,24,98,32,84,99,72,82,89,61,75,30,90,74,10,71,14,80,55,68,61,99,54,84,49,17,74,83,79,38,25,90,38,99,36,89,14,38,80,71,92,10,4,65,35,78,95,40,36,78,13,39,83,76,82,64,16,96,95,31,75,95,79,2,89,38,36,87,36,76,81,38,42,92,38,7,83,87,83,87,54,96,99,78,50,43,94,96,41,87,77,8,90,78,72,79,49,82,82,56,13,94,34,90,44,82,22,60,96,48,97,2,88,87,47,92,40,91,4,58,93,29,61,83,98,99,7,8,91,30,15,88,20,90,79,10,93,31,41,95,94,56,94,95,70,93,50,94,40,37,42,84,45,35,59,27,75,80,52,90,93,15,21,92,18,52,96,83,1,90,86,12,79,21,38,98,13,74,99,40,85,41,60,94,54,44,98,83,35,57,76,66,94,94,59,82,62,77,76,22,87,39,95,98,5,90,60,88,46,91,23,58,16,83,79,7,99,11,53,76,12,88,96,88,35,58,63,81,12,26,79,89,79,26,28,23,5,90,1,76,85,55,74,44,42,88,78,36,83,61,86,92,37,62,82,80,60,46,78,32,76,20,56,77,81,9,40,45,81,85,46,7,65,96,90,19,83,16,78,66,25,24,87,80,55,93,71,84,21,86,38,79,80,94,11,42,81,89,56,18,81,33,86,72,48,86,90,59,10,92,35,77,39,94,58,97,36,5,90,96,87,40,21,22,74,80,42,32,59,60,96,25,26,95,54,90,54,15,18,98,61,91,58,84,2,19,83,36,87,60,99,63,34,79,84,92,25,74,62,6,76,84,33,80,54,91,84,3,83,95,34,22,92,88,6,88,93,17,87,59,95,17,98,65,24,20,90,95,31,74,93,30,66,80,79,72,98,7,74,34,87,77,3,24,4,82,93,42,53,90,47,82,65,65,16,75,91,79,20,93,77,54,71,81,47,82,18,78,94,92,63,75,36,87,34,87,31,92,29,98,22,80,95,91,17,97,35,79,87,87,61,93,93,99,63,95,36,90,78,77,61,83,0,0,21,21,1,10,1,0,0,0,0,0,0";
    }
}