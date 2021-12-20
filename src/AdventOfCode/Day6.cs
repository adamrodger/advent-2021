using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 6
    /// </summary>
    public class Day6
    {
        // Originally a did the simulation manually because I R idiot, but obviously that doesn't work for part 2
        public long Part1(string[] input) => CountFish(input, 80);

        public long Part2(string[] input) => CountFish(input, 256);

        /// <summary>
        /// Count the number of fish present after the given number of days
        /// </summary>
        /// <param name="input">Puzzle input</param>
        /// <param name="days">Number of days to simulate</param>
        /// <returns>Number of fish</returns>
        private static long CountFish(string[] input, int days)
        {
            int[] numbers = input[0].Numbers<int>();
            long[] fish = new long[9];

            foreach (int n in numbers)
            {
                fish[n]++;
            }

            // each day all the 0s double to 8s and become 6s, everything else shifts down one, like a circular buffer
            for (int i = 0; i < days; i++)
            {
                long matured = fish[0];

                // shift everything down one
                fish[0] = fish[1];
                fish[1] = fish[2];
                fish[2] = fish[3];
                fish[3] = fish[4];
                fish[4] = fish[5];
                fish[5] = fish[6];
                fish[6] = fish[7];
                fish[7] = fish[8];

                fish[8] = matured; // they each spawn a new 8
                fish[6] += matured; // they reset to 6
            }

            return fish.Sum();
        }
    }
}
