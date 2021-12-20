using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 11
    /// </summary>
    public class Day11
    {
        private const int EnergyLimit = 9;
        private const int EnergyReset = 0;

        public int Part1(string[] input)
        {
            int[,] grid = input.ToGrid<int>();
            return Enumerable.Range(0, 100).Aggregate(0, (current, _) => current + Step(grid));
        }

        public int Part2(string[] input)
        {
            int[,] grid = input.ToGrid<int>();

            int round = 0;

            // keep going until they all flash together
            while (true)
            {
                round++;

                if (Step(grid) == grid.Length)
                {
                    return round;
                }
            }
        }

        /// <summary>
        /// Perform one step of the process
        /// </summary>
        /// <param name="grid">Current grid state (which is modified during the step)</param>
        /// <returns>Number of flashes in this step</returns>
        private static int Step(int[,] grid)
        {
            // increment the entire grid by 1
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[y, x]++;
                }
            }

            int flashes = 0;

            // check if anything is ready to flash
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] > EnergyLimit)
                    {
                        flashes += Flash((x, y), grid);
                    }
                }
            }

            return flashes;
        }

        /// <summary>
        /// Flash the given point
        /// </summary>
        /// <param name="point">Point to flash</param>
        /// <param name="grid">Grid to modify</param>
        /// <returns>Number of flashes caused (including cascades)</returns>
        private static int Flash(Point2D point, int[,] grid)
        {
            grid[point.Y, point.X] = EnergyReset;
            int flashes = 1;

            foreach (Point2D adjacent in grid.Adjacent8Positions(point))
            {
                if (grid[adjacent.Y, adjacent.X] == EnergyReset)
                {
                    // already flashed, don't increment again
                    continue;
                }

                grid[adjacent.Y, adjacent.X]++;

                // check if this causes a cascade flash
                if (grid[adjacent.Y, adjacent.X] > EnergyLimit)
                {
                    flashes += Flash(adjacent, grid);
                }
            }

            return flashes;
        }
    }
}
