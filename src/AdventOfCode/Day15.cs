using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 15
    /// </summary>
    public class Day15
    {
        public int Part1(string[] input)
        {
            int[,] grid = input.ToGrid<int>();
            return GetShortestPath(grid);
        }

        public int Part2(string[] input)
        {
            int[,] template = input.ToGrid<int>();
            int[,] grid = ExpandGrid(template);
            return GetShortestPath(grid);
        }

        /// <summary>
        /// Expand the grid to 5x its size, with added cost per replica
        /// </summary>
        /// <param name="template">Template grid</param>
        /// <returns>Expanded grid</returns>
        private static int[,] ExpandGrid(int[,] template)
        {
            int[,] grid = new int[template.GetLength(0) * 5, template.GetLength(1) * 5];

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    int relativeY = y % template.GetLength(0);
                    int relativeX = x % template.GetLength(1);

                    int modifierY = y / template.GetLength(0);
                    int modifierX = x / template.GetLength(1);
                    int modifier = modifierX + modifierY;

                    int value = template[relativeY, relativeX] + modifier;

                    if (value > 9)
                    {
                        value %= 9;
                    }

                    grid[y, x] = value;
                }
            }

            return grid;
        }

        /// <summary>
        /// Get the shortest path from the top-left of the grid to the bottom-right
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <returns>Shortest path</returns>
        private static int GetShortestPath(int[,] grid)
        {
            Point2D current = (0, 0);
            Point2D target = (grid.GetLength(1) - 1, grid.GetLength(0) - 1);

            // dijkstra's algorithm
            var open = new PriorityQueue<Point2D, int>();
            open.Enqueue(current, 0);

            var distances = new Dictionary<Point2D, int> { [current] = 0 };
            var closed = new HashSet<Point2D>();

            while (open.Count > 0)
            {
                current = open.Dequeue();
                closed.Add(current);

                if (current == target)
                {
                    return distances[target];
                }

                foreach (Point2D neighbour in grid.Adjacent4Positions(current))
                {
                    if (closed.Contains(neighbour))
                    {
                        continue;
                    }

                    int neighbourCost = distances[current] + grid[neighbour.Y, neighbour.X];

                    if (!distances.ContainsKey(neighbour) || distances[neighbour] > neighbourCost)
                    {
                        distances[neighbour] = neighbourCost;
                        open.Enqueue(neighbour, neighbourCost);
                    }
                }
            }

            throw new InvalidOperationException("There is no path from start to end");
        }
    }
}
