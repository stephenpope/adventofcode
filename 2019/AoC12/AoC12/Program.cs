using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AoC12
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = Data.rawData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var moons = lines.Select(x =>
                    Regex.Replace(x, "[^\\d-,]", string.Empty)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(m => new Moon(int.Parse(m[0]), int.Parse(m[1]), int.Parse(m[2]))).ToList();

            var step = 0;
            long cycleX = -1;
            long cycleY = -1;
            long cycleZ = -1;

            while(true)
            {
                foreach (var moon in moons)
                {
                    foreach (var otherMoon in moons.Where(x => x != moon))
                    {
                        var changeX = Math.Sign(otherMoon.X - moon.X);
                        var changeY = Math.Sign(otherMoon.Y - moon.Y);
                        var changeZ = Math.Sign(otherMoon.Z - moon.Z);

                        moon.XVelocity += changeX;
                        moon.YVelocity += changeY;
                        moon.ZVelocity += changeZ;
                    }
                }

                foreach (var moon in moons)
                {
                    moon.X += moon.XVelocity;
                    moon.Y += moon.YVelocity;
                    moon.Z += moon.ZVelocity;
                }
                
                step++;

                if (step == 1000)
                {
                    Console.WriteLine("Step: " + step + " / Total Energy : " + moons.Sum(x => x.TotalEnergy));
                }
                
                //PART II

                if (cycleX == -1 && moons.All(x => x.XVelocity == 0))
                {
                    cycleX = step;
                }

                if (cycleY == -1 && moons.All(x => x.YVelocity == 0))
                {
                    cycleY = step;
                }

                if (cycleZ == -1 && moons.All(x => x.ZVelocity == 0))
                {
                    cycleZ = step;
                }

                if (cycleX == -1 || cycleY == -1 || cycleZ == -1) continue;
                
                Console.WriteLine("Cycle :");
                Console.WriteLine(Maths.LeastCommonMultiple(new[] { cycleX, cycleY, cycleZ }) * 2);
                break;
            }
        }
    }

    public static class Data
    {
        public static string testDataOne = @"
<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";
        
        public static string testDataTwo = @"
<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";
        
        public static string rawData = @"
<x=5, y=13, z=-3>
<x=18, y=-7, z=13>
<x=16, y=3, z=4>
<x=0, y=8, z=8>";
    }
}