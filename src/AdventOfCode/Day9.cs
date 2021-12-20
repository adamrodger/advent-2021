using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 9
    /// </summary>
    public class Day9
    {
        public int Part1(string[] input)
        {
            int result = 0;
            char[,] grid = input.ToGrid();

            grid.ForEach((x, y, c) =>
            {
                if (grid.Adjacent4(x, y).All(a => a > c))
                {
                    result += int.Parse(c.ToString()) + 1;
                }
            });

            return result;
        }

        public int Part2(string[] input)
        {
            char[,] grid = input.ToGrid();

            var visited = new HashSet<(int x, int y)>();
            var queue = new Queue<(int x, int y)>();
            var basins = new List<List<(int x, int y)>>();

            // mark all the high points off
            grid.ForEach((x, y, c) =>
            {
                if (c == '9')
                {
                    visited.Add((x, y));
                }
            });

            var deltas = new[]
            {
                (-1, 0),
                (0, -1),
                (1, 0),
                (0, 1)
            };

            while (visited.Count < grid.Length)
            {
                // start the next basin
                var basin = new List<(int x, int y)>();
                basins.Add(basin);

                EnqueueNext(grid, visited, queue);

                // flood-fill this basin until you hit all the 'walls' (represented by 9s)
                while (queue.Any())
                {
                    (int x, int y) = queue.Dequeue();

                    if (visited.Contains((x, y)))
                    {
                        continue;
                    }

                    basin.Add((x, y));
                    visited.Add((x, y));

                    foreach ((int dx, int dy) in deltas)
                    {
                        int x1 = x + dx;
                        int y1 = y + dy;

                        if (x1 >= 0 && x1 < grid.GetLength(1)
                         && y1 >= 0 && y1 < grid.GetLength(0)
                         && grid[y, x] != '9')
                        {
                            queue.Enqueue((x1, y1));
                        }
                    }
                }
            }

            // multiply the size of the 3 biggest basins
            int[] biggest = basins.Select(b => b.Count).OrderByDescending(b => b).Take(3).ToArray();
            return biggest[0] * biggest[1] * biggest[2];
        }

        /// <summary>
        /// Enqueue the next cell which has not been visited yet, i.e. to start a new basin
        /// </summary>
        private static void EnqueueNext(char[,] grid, HashSet<(int x, int y)> visited, Queue<(int x, int y)> queue)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (!visited.Contains((x, y)))
                    {
                        queue.Enqueue((x, y));
                        return;
                    }
                }
            }
        }
    }
}
