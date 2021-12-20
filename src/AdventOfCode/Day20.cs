using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 20
    /// </summary>
    public class Day20
    {
        /// <summary>
        /// Lookup of pixel delta to the index value to add if it's lit
        /// </summary>
        private static readonly Dictionary<Point2D, int> Deltas = new()
        {
            [(-1, -1)] = 0b100_000_000,
            [( 0, -1)] = 0b010_000_000,
            [( 1, -1)] = 0b001_000_000,
            [(-1,  0)] = 0b000_100_000,
            [( 0,  0)] = 0b000_010_000,
            [( 1,  0)] = 0b000_001_000,
            [(-1,  1)] = 0b000_000_100,
            [( 0,  1)] = 0b000_000_010,
            [( 1,  1)] = 0b000_000_001,
        };

        public int Part1(string[] input)
        {
            (string lookup, IReadOnlySet<Point2D> lit) = Parse(input);

            lit = Transform(lookup, lit, 0);
            lit = Transform(lookup, lit, 1);

            return lit.Count;
        }

        public int Part2(string[] input)
        {
            (string lookup, IReadOnlySet<Point2D> lit) = Parse(input);

            for (int i = 0; i < 50; i++)
            {
                lit = Transform(lookup, lit, i);
            }

            return lit.Count;
        }

        /// <summary>
        /// Parse the input to a lookup key and a set of the lit pixels
        /// </summary>
        private static (string lookup, IReadOnlySet<Point2D> lit) Parse(IReadOnlyCollection<string> input)
        {
            string lookup = input.First();

            HashSet<Point2D> lit = new HashSet<Point2D>();

            foreach ((string line, int y) in input.Skip(2).Select((line, i) => (line, i)))
            {
                foreach ((char c, int x) in line.Select((c, i) => (c, i)))
                {
                    if (c == '#')
                    {
                        lit.Add((x, y));
                    }
                }
            }

            return (lookup, lit);
        }

        /// <summary>
        /// Transform the input set of lit pixels into an output set
        /// </summary>
        /// <param name="lookup">Translation lookup</param>
        /// <param name="input">Input lit pixels</param>
        /// <param name="i">Transformation iteration</param>
        /// <returns>Output lit pixels</returns>
        private static IReadOnlySet<Point2D> Transform(string lookup, IReadOnlySet<Point2D> input, int i)
        {
            var output = new HashSet<Point2D>();

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (Point2D point in input)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);

                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            /*
             * Disco mode
             *
             * If lookup[0] is lit (i.e. entirely dark regions become lit) then we're in disco mode.
             * Each odd iteration the entire infinite region will be lit, then next iteration it'll
             * all go dark again.
             *
             * If lookup[0] is unlit then we'll never be in disco mode because dark regions stay dark
             */
            bool discoMode = lookup[0] == '#' && i % 2 == 1;

            // grow the image by 1 in each direction
            for (int y = minY - 1; y <= maxY + 1; y++)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    int index = 0;

                    foreach ((Point2D delta, int value) in Deltas)
                    {
                        Point2D check = (x + delta.X, y + delta.Y);

                        bool lit;

                        if (check.X < minX || check.Y < minY || check.X > maxX || check.Y > maxY)
                        {
                            // we hit a border, so use default
                            lit = discoMode;
                        }
                        else
                        {
                            lit = input.Contains(check);
                        }

                        if (lit)
                        {
                            index += value;
                        }
                    }

                    if (lookup[index] == '#')
                    {
                        output.Add((x, y));
                    }
                }
            }

            return output;
        }
    }
}
