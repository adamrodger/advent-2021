using System.Collections.Generic;
using System.Linq;
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

            int[,] grid = new int[template.GetLength(0) * 5, template.GetLength(1) * 5];

            // blow the grid up 5x
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

                    grid[y,x] = value;
                }
            }

            return GetShortestPath(grid);
        }

        /// <summary>
        /// Get the shortest path from the top-left of the grid to the bottom-right
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <returns>Shortest path</returns>
        private static int GetShortestPath(int[,] grid)
        {
            var graph = new Graph<Point2D>(Graph<Point2D>.ManhattanDistanceHeuristic);

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Point2D current = (x, y);
                    int currentCost = grid[y, x];

                    foreach (Point2D neighbour in grid.Adjacent4Positions(x, y))
                    {
                        int cost = grid[neighbour.Y, neighbour.X];
                        graph.AddVertex(current, neighbour, cost);

                        graph.AddVertex(neighbour, current, currentCost);
                    }
                }
            }

            List<(Point2D node, int distance)> path = graph.GetShortestPath((0, 0), (grid.GetLength(1) - 1, grid.GetLength(0) - 1));

            return path.Last().distance;
        }
    }
}
