using System;
using System.Linq;

namespace AoC16
{
    class Program
    {
        static void Main(string[] args)
        {
            var signal = Data.rawData;
            
            //Console.WriteLine('5' - '0'); // Little trick : char(5) minus char(0) = int(5)
            
            Console.WriteLine("Part I  - First Eight Digits : " + CalculatePartOne(signal, 100));
            Console.WriteLine("Part II - First Eight Digits : " + CalculatePartTwo(signal, 100));
        }

        private static string CalculatePartOne(string signal, int phases)
        {
            for(var p = 0; p < phases; p++) 
            {
                var output = string.Empty;

                foreach(var signalIndex in signal.Select((v,i) => i)) //Use 'index' of the string length to generate the pattern
                {
                    var sum = signal
                        .Select((value, index) => new {index, value})
                        .Sum(x => (x.value - '0') * new[] {0, 1, 0, -1}[(x.index + 1) / (signalIndex + 1) % 4]);

                    if (sum < 0)
                    {
                        sum = Math.Abs(sum); // Make negative numbers absolute.
                    }

                    output += sum % 10; //Only use the last (far-right) digit
                }

                signal = output;
            }

            return signal[new Range(0, 8)];
        }

        // 1*1  + 2*0  + 3*-1 + 4*0  + 5*1  + 6*0  + 7*-1 + 8*0  = 4
        // 1*0  + 2*1  + 3*1  + 4*0  + 5*0  + 6*-1 + 7*-1 + 8*0  = 8
        // 1*0  + 2*0  + 3*1  + 4*1  + 5*1  + 6*0  + 7*0  + 8*0  = 2
        // 1*0  + 2*0  + 3*0  + 4*1  + 5*1  + 6*1  + 7*1  + 8*0  = 2
        // 1*0  + 2*0  + 3*0  + 4*0  + 5*1  + 6*1  + 7*1  + 8*1  = 6 <--
        // 1*0  + 2*0  + 3*0  + 4*0  + 5*0  + 6*1  + 7*1  + 8*1  = 1 <--
        // 1*0  + 2*0  + 3*0  + 4*0  + 5*0  + 6*0  + 7*1  + 8*1  = 5 <--
        // 1*0  + 2*0  + 3*0  + 4*0  + 5*0  + 6*0  + 7*0  + 8*1  = 8 <--
        //
        // After 1 phase: 48226158
        //                    ^^^^
        
        // ...  = 4
        // ...  = 8
        // ...  = 2
        // ...  = 2
        // 5 + 6 + 7 + 8  = 6 (abs(26) % 10)
        // 6 + 7 + 8  = 1 (abs(21) % 10)
        // 7 + 8  = 5 (abs(15) % 10)
        // 8  = 8 (abs(8) % 10)
        // https://github.com/mebeim/aoc/blob/master/2019/README.md#day-16---flawed-frequency-transmission

        private static string CalculatePartTwo(string signal, int phase)
        {
            var offset = int.Parse(signal[new Range(0,7)]);
            var fullSignal = string.Concat(Enumerable.Repeat(signal, 10000));
            var output = fullSignal
                .Skip(offset)
                .Select(character => (int) character)
                .ToList();

            for(var p = 0; p < phase; p++)
            {
                var sum = 0;
                
                //Work backwards from the last digit adding digits together..
                for(var i = output.Count - 1; i >= 0; i--) 
                {
                    sum += output[i];
                    output[i] = sum % 10;
                }
            }

            return string.Concat(output.Take(8));
        }
    }

    public static class Data
    {
        public static string testOne   = "12345678";
        public static string testTwo   = "80871224585914546619083218645595";
        public static string testThree = "19617804207202209144916044189917";
        public static string testFour  = "69317163492948606335995924319873";

        public static string testFive  = "03036732577212944063491565474664";
        public static string testSix   = "02935109699940807407585447034323";
        public static string testSeven = "03081770884921959731165446850517";
        
        public static string rawData = "59728776137831964407973962002190906766322659303479564518502254685706025795824872901465838782474078135479504351754597318603898249365886373257507600323820091333924823533976723324070520961217627430323336204524247721593859226704485849491418129908885940064664115882392043975997862502832791753443475733972832341211432322108298512512553114533929906718683734211778737511609226184538973092804715035096933160826733751936056316586618837326144846607181591957802127283758478256860673616576061374687104534470102346796536051507583471850382678959394486801952841777641763547422116981527264877636892414006855332078225310912793451227305425976335026620670455240087933409";
    }
}