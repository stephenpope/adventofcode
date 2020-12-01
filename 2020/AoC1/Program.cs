using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AoC1
{
    class Program
    {
        private static readonly int[] SampleInputValues =
        {
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        private static readonly HashSet<int> InputValues = new()
        {
            1695,
            1157,
            1484,
            1717,
            622,
            1513,
            1924,
            63,
            1461,
            1971,
            1382,
            1587,
            1913,
            1665,
            1464,
            1914,
            1637,
            1527,
            1424,
            1361,
            1187,
            272,
            1909,
            1448,
            1623,
            1164,
            1931,
            1646,
            1096,
            1655,
            1962,
            1961,
            1694,
            1792,
            1989,
            1616,
            138,
            1887,
            1357,
            1965,
            1085,
            308,
            2007,
            1254,
            1179,
            1124,
            1719,
            1467,
            1928,
            1630,
            1676,
            1359,
            1241,
            1511,
            1413,
            1656,
            1818,
            1919,
            1422,
            1745,
            1208,
            1609,
            1544,
            1775,
            1154,
            1057,
            1440,
            1242,
            1202,
            1266,
            1305,
            1836,
            1760,
            1730,
            1396,
            1315,
            1496,
            1964,
            1300,
            1195,
            1583,
            1607,
            1743,
            1682,
            1453,
            1848,
            1320,
            1601,
            954,
            1473,
            1847,
            1486,
            1853,
            1668,
            1342,
            1087,
            1139,
            1349,
            1568,
            1728,
            1420,
            1233,
            1073,
            1376,
            1658,
            1477,
            1871,
            1958,
            1950,
            1503,
            1758,
            1474,
            1203,
            1336,
            1981,
            1309,
            1618,
            1846,
            1974,
            1940,
            1333,
            1119,
            1756,
            1918,
            961,
            1307,
            1375,
            1346,
            1611,
            1284,
            84,
            1754,
            1608,
            2010,
            1341,
            1136,
            1218,
            1882,
            1911,
            1288,
            1930,
            1749,
            1952,
            1556,
            1757,
            1761,
            1112,
            1963,
            1186,
            1373,
            1622,
            1973,
            1330,
            1508,
            1222,
            1226,
            1389,
            1679,
            1584,
            1237,
            1563,
            1763,
            1998,
            1293,
            1642,
            95,
            1661,
            1674,
            1100,
            1262,
            1895,
            1548,
            1400,
            1205,
            1435,
            1156,
            1034,
            1577,
            1701,
            1198,
            1173,
            1500,
            1858,
            1809,
            1780,
            1412,
            1982,
            1070,
            1523,
            1776,
            1598,
            1113,
            1144,
            1777,
            1313,
            1102,
            1999,
            1405,
            1784,
            1196,
        };

        static void Main(string[] args)
        {
            // Why 2020-x ? Locate the other half of the equation quickly (one loop) by eliminating invalid values - in theory there are only 2 values that match.
            // Why Aggregate ? Its an accumulator .. (1 * value1) * value2) => ((1 * 138) * 1882)- Again assuming there are only 2 valid values.

            var sw = Stopwatch.StartNew();

            Console.Write("Part One [LINQ] - Result: " + InputValues
                .Where(x => InputValues.Contains(2020 - x))
                .Aggregate(1, (a, b) => a * b));

            sw.Stop();
            Console.WriteLine($" - [{sw.ElapsedMilliseconds} ms => {sw.ElapsedTicks.ToString("0,0",CultureInfo.InvariantCulture)} ticks]");

            // Why FirstOrDefault ? Another .Where() will return an array - this assumes only 1 correct answer so we return just that!

            sw.Restart();

            Console.Write("Part Two [LINQ] - Result: " + InputValues
                .Where(o => InputValues.FirstOrDefault(c => InputValues.Contains(2020 - o - c)) > 0)
                .Aggregate(1, (a, b) => a * b));

            sw.Stop();
            Console.WriteLine($" - [{sw.ElapsedMilliseconds} ms => {sw.ElapsedTicks.ToString("0,0",CultureInfo.InvariantCulture)} ticks]");

            // EXTREME OPTIMIZATION ! Knowing we have only 2 results we can do stupid things in the name of speed ..

            var result = new int[2];
            var accumulator = 0;

            sw.Restart();

            foreach (var value in InputValues)
            {
                foreach (var subValue in InputValues)
                {
                    if (2020 - value != subValue) continue;

                    result[accumulator] = subValue;
                    accumulator += 1;
                }
            }
            
            var partOneResult = result[0] * result[1];

            sw.Stop();

            Console.Write("Part One [RAW] - Result: " + partOneResult);
            Console.WriteLine($" - [{sw.ElapsedMilliseconds} ms => {sw.ElapsedTicks.ToString("0,0",CultureInfo.InvariantCulture)} ticks]");
            
            sw.Restart();

            // Trick using C#7 local functions .. exits from nested foreach by using return.

            int DoWork()
            {
                foreach (var value in SampleInputValues)
                {
                    foreach (var subValue in SampleInputValues)
                    {
                        foreach (var subSubValue in SampleInputValues)
                        {
                            if (2020 - value - subValue - subSubValue != 0) continue;

                            return value * subValue * subSubValue;
                        }
                    }
                }

                return 0;
            };

            var partTwoResult = DoWork();

            sw.Stop();

            Console.Write("Part Two [RAW] - Result: " + partTwoResult);
            Console.WriteLine($" - [{sw.ElapsedMilliseconds} ms => {sw.ElapsedTicks.ToString("0,0",CultureInfo.InvariantCulture)} ticks]");
        }
    }
}