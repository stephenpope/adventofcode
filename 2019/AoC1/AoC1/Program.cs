using System;
using System.Linq;

namespace AoC1
{
    class Program
    {
        private static readonly string RawInput = @"106985
            113927
            107457
            106171
            69124
            59906
            66420
            149336
            73783
            120127
            139486
            108698
            104091
            103032
            108609
            136293
            144735
            55381
            98823
            103981
            140684
            114482
            133925
            111247
            110833
            92252
            87396
            79730
            61395
            82572
            72403
            140763
            57088
            63457
            65523
            50148
            134758
            93447
            85513
            132927
            139159
            141579
            94444
            56997
            137128
            107930
            67607
            108837
            120206
            79441
            99839
            137404
            140502
            67274
            108736
            97302
            76561
            107804
            134306
            52820
            89632
            101473
            65001
            57399
            82858
            60577
            82043
            144783
            101606
            138900
            68246
            118774
            129919
            99394
            80009
            107404
            121503
            119232
            108157
            117965
            112025
            139205
            126336
            143985
            58894
            93020
            136732
            100535
            144090
            134414
            109049
            105714
            111654
            50677
            77622
            53398
            133851
            71166
            115935
            94067";

        static void Main(string[] args)
        {
            var inputArray = RawInput.Split('\n').Select(int.Parse);

            var totalOne = inputArray.Sum(module => CalculateFuel(module, false));

            Console.WriteLine("Part 1 Total: " + totalOne);
            
            var totalTwo = inputArray.Sum(module => CalculateFuel(module, true));

            Console.WriteLine("Part 2 Total: " + totalTwo);
        }

        private static int CalculateFuel(int input, bool reFeed)
        {
            //var fuelNeeded = (int) Math.Round((double) (input / 3)) - 2;
            var fuelNeeded = input / 3 - 2; //Not sure why this doesnt blow up !

            // Part1 vs Part 2
            if (!reFeed)
            {
                return fuelNeeded;
            }
            
            //Less than zero = return zero
            if (fuelNeeded <= 0)
            {
                return 0;
            }

            //Feed result back as input..
            return fuelNeeded + CalculateFuel(fuelNeeded, reFeed);
        }
    }
}