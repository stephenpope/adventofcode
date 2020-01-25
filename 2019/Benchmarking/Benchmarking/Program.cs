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
        private int one;

        [GlobalSetup]
        public void Setup()
        {
            one = SetPoint(1, 1);
        }

        public static (int x, int y) GetPointTuple(int node)
        {
            return (node & 255,(node >> 8) & 255);
        }

        public static int[] GetPointArray(int node)
        {
            return new[] {node & 255, (node >> 8) & 255};
        }

        public static int SetPoint(int x, int y)
        {
            return x | (y << 8);
        }

        [Benchmark]
        public (int x, int y) GetTuple() => GetPointTuple(one);

        [Benchmark]
        public int[] GetArray() => GetPointArray(one);

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
}