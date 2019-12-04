using System;
using System.Linq;

namespace AoC4
{
    class Program
    {
        static void Main(string[] args)
        {
            var range = new Range(109165,576723);
            var matchCount = 0;
            
            for (var i = range.Start.Value; i <= range.End.Value; i++)
            {
                var password = i.ToString().Select(x => int.Parse(x.ToString())).ToArray();
                
                if (TestPassword(password, true))
                {
                    matchCount++;
                }
            }
            
            Console.WriteLine("Total:" + matchCount);
        }

        private static bool TestPassword(int[] input, bool strict)
        {
            
            //Copy (to compare)
            var original = new int[input.Length];
            Array.Copy(input, original, input.Length);
            //Sort
            Array.Sort(input);

            // If the sorted array matches the original then that number is in the correct (ascending) order
            if (!input.SequenceEqual(original)) return false;
            
            if (strict) //For Part2
            {
                return input.Any(digit => input.Count(x => x == digit) == 2); //Clusters must be 2    
            }
                
            return input.Any(digit => input.Count(x => x == digit) >= 2); //Any cluster of 2 or more

        }
    }
}