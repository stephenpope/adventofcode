using System;
using System.Collections.Generic;

namespace AoC12
{
    public static class Maths
    {
        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                var remainder = a%b;
                a = b;
                b = remainder;
            }

            return Math.Abs(a);
        }
        
        public static long LeastCommonMultiple(long a, long b)
        {
            if ((a == 0) || (b == 0))
            {
                return 0;
            }

            return Math.Abs(a/GreatestCommonDivisor(a, b)*b);
        }

        public static long LeastCommonMultiple(IList<long> integers)
        {
            if (null == integers)
            {
                throw new ArgumentNullException(nameof(integers));
            }

            if (integers.Count == 0)
            {
                return 1;
            }

            var lcm = Math.Abs(integers[0]);

            for (var i = 1; i < integers.Count; i++)
            {
                lcm = LeastCommonMultiple(lcm, integers[i]);
            }

            return lcm;
        }
    }
}