using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;

namespace Benchmarking
{
    public class Program
    {
        private Point[] one;
        private Point[] two;

        [GlobalSetup]
        public void Setup()
        {
            one = new Point[4];
            one[0] = new Point(1,1);
            
            two = new Point[4];
        }

        // [Benchmark]
        // public bool CompareFor() => one.BitEquals(two);
        //
        // [Benchmark]
        // public bool CompareLinq() => three.SequenceEqual(four);

        [Benchmark]
        public void Copy() => Array.Copy(one, two, one.Length);

        [Benchmark]
        public void Clone() => one.AsSpan().CopyTo(two);

        static void Main(string[] args)
        { 
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
    
}