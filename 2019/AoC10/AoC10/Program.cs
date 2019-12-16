using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC10
{
    public class Asteroid
    {
        public int X { get; }
        public int Y { get; }

        public Asteroid(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class LocationData
    {
        public double Angle { get; set; }
        public double Distance { get; set; }
        public Asteroid StartRoid { get; set; }
        public Asteroid EndRoid { get; set; }
        
        public bool Destroyed { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var y = 0;

            var asteroids = new List<Asteroid>();

            foreach (var line in Data.rawData.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        asteroids.Add(new Asteroid(x, y));
                    }
                }

                y++;
            }

            var (startRoid, highestHit) = FindBestLocation(asteroids);
            
            Console.WriteLine($"Best: {startRoid.X}/{startRoid.Y} => {highestHit}");

            var fullLocationData = asteroids
                .Where(asteroid => startRoid != asteroid)
                .Select(endRoid => new LocationData
                {
                    StartRoid = startRoid, 
                    EndRoid = endRoid, 
                    Angle = Math.Atan2(startRoid.X - endRoid.X, startRoid.Y - endRoid.Y), 
                    Distance = Math.Abs((double) (startRoid.X - endRoid.X) * -(startRoid.X - endRoid.X) + (startRoid.Y - endRoid.Y) * -(startRoid.Y - endRoid.Y))
                })
                .ToList();

            Console.WriteLine("Asteroid: " + startRoid.X + "," + startRoid.Y);

            var shotCount = 0; //Keep track of number of shots.
            
            //Loop through until all are destroyed ..
            while (fullLocationData.Any(x => x.Destroyed == false))
            {
                var firingSet = new List<LocationData>();

                foreach (var data in fullLocationData.OrderBy(x => (180 - x.Angle) % 180).ThenBy(x => x.Distance))
                {
                    if (data.Destroyed) continue;
                    if (firingSet.Any(x => x.Angle == data.Angle && x.Distance < data.Distance)) continue; //Pick nearest first and work outwards each time
                    data.Destroyed = true;
                    firingSet.Add(data);
                }

                foreach (var target in firingSet)
                {
                    shotCount++;
                    Console.WriteLine("PEW! => " + "(" + shotCount + ") " + target.EndRoid.X + "," + target.EndRoid.Y + " => " + (target.EndRoid.X * 100 + target.EndRoid.Y));
                }
            }
        }

        private static (Asteroid bestRoid, int highestHit) FindBestLocation(IEnumerable<Asteroid> asteroids)
        {
            var highest = 0;
            Asteroid bestRoid = null;

            foreach (var startRoid in asteroids)
            {
                var angles = new HashSet<double>();

                foreach (var endRoid in asteroids.Where(asteroid => startRoid != asteroid))
                {
                    //Find angle between two points - if another asteroid has the same angle then its blocked (so only count once (hashset))
                    angles.Add(Math.Atan2(startRoid.Y - endRoid.Y, startRoid.X - endRoid.X));
                }

                if (angles.Count <= highest) continue;

                highest = angles.Count;
                bestRoid = startRoid;
            }

            return (bestRoid, highest);
        }
    }

    public static class Data
    {
        public static string TestOne = @"
.#..#
.....
#####
....#
...##"; //3,4(8) 3,2/4,0/4,2/4,3/4,4/0,2/1,2/2,2/1,0

        public static string TestTwo = @"
......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####"; //5,8(33)


        public static string TestThree = @"
#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###."; //1,2(35)

        public static string TestFour = @"
.#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..
"; //6,3 (41)

        public static string TestFive = @"
.#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##"; //11,13(210)

        public static string rawData = @"
.#..#..##.#...###.#............#.
.....#..........##..#..#####.#..#
#....#...#..#.......#...........#
.#....#....#....#.#...#.#.#.#....
..#..#.....#.......###.#.#.##....
...#.##.###..#....#........#..#.#
..#.##..#.#.#...##..........#...#
..#..#.......................#..#
...#..#.#...##.#...#.#..#.#......
......#......#.....#.............
.###..#.#..#...#..#.#.......##..#
.#...#.................###......#
#.#.......#..####.#..##.###.....#
.#.#..#.#...##.#.#..#..##.#.#.#..
##...#....#...#....##....#.#....#
......#..#......#.#.....##..#.#..
##.###.....#.#.###.#..#..#..###..
#...........#.#..#..#..#....#....
..........#.#.#..#.###...#.....#.
...#.###........##..#..##........
.###.....#.#.###...##.........#..
#.#...##.....#.#.........#..#.###
..##..##........#........#......#
..####......#...#..........#.#...
......##...##.#........#...##.##.
.#..###...#.......#........#....#
...##...#..#...#..#..#.#.#...#...
....#......#.#............##.....
#......####...#.....#...#......#.
...#............#...#..#.#.#..#.#
.#...#....###.####....#.#........
#.#...##...#.##...#....#.#..##.#.
.#....#.###..#..##.#.##...#.#..##";
    }
}