using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
/// <summary>
/// Solver for Day 11
/// </summary>
    public class Day11
    {
        public int Part1(string[] input)
        {
            int[,] grid = ParseGrid(input);
            return Enumerable.Range(0, 100).Aggregate(0, (current, _) => current + Step(grid));
        }

        public int Part2(string[] input)
        {
            int[,] grid = ParseGrid(input);

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
        /// Parse the grid
        /// </summary>
        /// <param name="input">Input lines</param>
        /// <returns>Grid</returns>
        private static int[,] ParseGrid(string[] input)
        {
            // y,x remember, not x,y
            int[,] grid = new int[input.Length, input[0].Length];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    grid[y, x] = int.Parse(input[y][x].ToString());
                }
            }

            return grid;
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

            Queue<Point2D> toFlash = new Queue<Point2D>();
            HashSet<Point2D> flashed = new HashSet<Point2D>();

            // queue up any initial flashes
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] > 9)
                    {
                        toFlash.Enqueue((x, y));
                        flashed.Add((x, y));
                    }
                }
            }

            while (toFlash.Any())
            {
                Point2D flash = toFlash.Dequeue();

                // increment all the adjacent ones, and see if that causes cascade flashes
                foreach (Point2D adjacent in grid.Adjacent8Positions(flash.X, flash.Y))
                {
                    if (flashed.Contains(adjacent))
                    {
                        continue;
                    }
                    
                    grid[adjacent.Y, adjacent.X]++;

                    // check if this causes a cascade flash
                    if (grid[adjacent.Y, adjacent.X] > 9)
                    {
                        toFlash.Enqueue(adjacent);
                        flashed.Add(adjacent);
                    }
                }

                // reset flashed octopus back to 0
                grid[flash.Y, flash.X] = 0;
            }

            return flashed.Count;
        }
    }
}
