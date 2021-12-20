using System;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 17
    /// </summary>
    public class Day17
    {
        public int Part1(string[] input)
        {
            (int minX, int maxX, int minY, int maxY) = ParseInput(input);

            int biggestY = int.MinValue;

            foreach (int dx in Enumerable.Range(0, maxX + 1))
            {
                foreach (int dy in Enumerable.Range(minY, Math.Abs(minY) * 2))
                {
                    (bool hit, int highest) = HitsTarget(dx, dy, minX, maxX, minY, maxY);

                    if (hit)
                    {
                        biggestY = Math.Max(biggestY, highest);
                    }
                }
            }

            return biggestY;
        }

        public int Part2(string[] input)
        {
            (int minX, int maxX, int minY, int maxY) = ParseInput(input);

            int result = 0;

            foreach (int dx in Enumerable.Range(0, maxX + 1))
            {
                foreach (int dy in Enumerable.Range(minY, Math.Abs(minY) * 2))
                {
                    (bool hit, _) = HitsTarget(dx, dy, minX, maxX, minY, maxY);

                    if (hit)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parse the input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>target bounds</returns>
        private static (int minX, int maxX, int minY, int maxY) ParseInput(string[] input)
        {
            var numbers = input[0].Numbers<int>();

            int minX = numbers[0];
            int maxX = numbers[1];
            int minY = numbers[2];
            int maxY = numbers[3];

            return (minX, maxX, minY, maxY);
        }

        /// <summary>
        /// Check if the given x/y velocity starting points cause the probe to enter the target area
        /// </summary>
        /// <param name="vx">Starting X velocity</param>
        /// <param name="vy">Starting y velocity</param>
        /// <param name="minX">Target min X</param>
        /// <param name="maxX">Target max X</param>
        /// <param name="minY">Target min Y</param>
        /// <param name="maxY">Target max Y</param>
        /// <returns>If the probe enters the target zone, and the highest Y point it ever reached</returns>
        private static (bool hit, int highest) HitsTarget(int vx, int vy, int minX, int maxX, int minY, int maxY)
        {
            int x = 0;
            int y = 0;
            int highest = 0;

            while (x <= maxX && y >= minY)
            {
                x += vx;
                y += vy;

                highest = Math.Max(y, highest);

                vx = Math.Max(0, vx - 1); // don't let vx go negative
                vy--;

                if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                {
                    return (true, highest);
                }
            }

            return (false, 0);
        }
    }
}
