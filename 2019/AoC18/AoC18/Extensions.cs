using System.Collections;

namespace AoC18
{
    public static class Extensions
    {
        public static bool BitEquals(this BitArray current, BitArray other)
        {
            for (var i = 0; i < current.Length; i++)
            {
                if (current[i] != other[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}