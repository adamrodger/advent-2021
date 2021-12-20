using System;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 7
    /// </summary>
    public class Day7
    {
        public int Part1(string[] input)
        {
            int[] crabs = input[0].Numbers<int>();
            var min = crabs.Min();
            var max = crabs.Max();

            int result = int.MaxValue;

            for (int i = min; i < max; i++)
            {
                int fuel = crabs.Select(c => Math.Abs(c - i)).Sum();

                if (fuel < result)
                {
                    result = fuel;
                }
            }

            return result;
        }

        public int Part2(string[] input)
        {
            int[] crabs = input[0].Numbers<int>();
            var min = crabs.Min();
            var max = crabs.Max();

            int result = int.MaxValue;

            for (int i = min; i < max; i++)
            {
                int fuel = crabs.Select(c => Math.Abs(c - i)).Select(p => p * (p + 1) / 2).Sum();

                if (fuel < result)
                {
                    result = fuel;
                }
            }

            return result;
        }
    }
}
