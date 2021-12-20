using System;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 8
    /// </summary>
    public class Day8
    {
        public int Part1(string[] input)
        {
            return input.Select(i => i.Split(' '))
                        .Select(i => i.Skip(11)) // skip to output
                        .SelectMany(i => i)
                        .Count(i => i.Length is 2 or 3 or 4 or 7);
        }

        public int Part2(string[] input)
        {
            int result = 0;

            foreach (string line in input)
            {
                string[] parts = line.Split(" ");
                string[] source = parts.Take(10).ToArray();
                string[] dest = parts.Skip(11).ToArray();

                result += CalculateOutput(source, dest);
            }

            return result;
        }

        private static int CalculateOutput(string[] source, string[] dest)
        {
            // these numbers have unique lengths
            string one = source.First(s => s.Length == 2);
            string four = source.First(s => s.Length == 4);

            int result = 0;
            int multiplier = (int)Math.Pow(10, dest.Length - 1);

            foreach (string d in dest)
            {
                // calculate each number from its length and its mask to the known layouts of 4 and 1
                int value = (d.Length, d.Intersect(four).Count(), d.Intersect(one).Count()) switch
                {
                    (2, _, _) => 1,
                    (3, _, _) => 7,
                    (4, _, _) => 4,
                    (7, _, _) => 8,
                    (5, 3, 2) => 3,
                    (5, 3, 1) => 5,
                    (5, 2, _) => 2,
                    (6, 3, 2) => 0,
                    (6, 3, 1) => 6,
                    (6, 4, _) => 9,
                    _ => throw new InvalidOperationException($"Unrecognised character: {d}")
                };

                result += value * multiplier;
                multiplier /= 10;
            }

            return result;
        }
    }
}
