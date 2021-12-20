using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 5
    /// </summary>
    public class Day5
    {
        public int Part1(string[] input)
        {
            return Run(input, Part.One);
        }

        public int Part2(string[] input)
        {
            return Run(input, Part.Two);
        }

        private static int Run(string[] input, Part part)
        {
            Dictionary<Point2D, int> points = new Dictionary<Point2D, int>();

            foreach (string line in input.Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                int[] numbers = line.Numbers<int>();

                (int x0, int y0, int x1, int y1) = (numbers[0], numbers[1], numbers[2], numbers[3]);

                if (x0 == x1)
                {
                    // vertical
                    for (int i = Math.Min(y0, y1); i <= Math.Max(y0, y1); i++)
                    {
                        Point2D point = (x0, i);
                        points[point] = points.GetOrCreate(point) + 1;
                    }
                }
                else if (y0 == y1)
                {
                    // horizontal
                    for (int i = Math.Min(x0, x1); i <= Math.Max(x0, x1); i++)
                    {
                        Point2D point = (i, y0);
                        points[point] = points.GetOrCreate(point) + 1;
                    }
                }
                else if (part == Part.Two) // part 1 ignores diagonals
                {
                    // diagonal
                    (int dx, int dy) = (x0 < x1, y0 < y1) switch
                    {
                        (true, true)   => ( 1,  1), // right and down
                        (true, false)  => ( 1, -1), // right and up
                        (false, true)  => (-1,  1), // left and down
                        (false, false) => (-1, -1)  // left and up
                    };

                    Point2D current = (x0, y0);
                    Point2D target = (x1 + dx, y1 + dy);

                    while (current != target)
                    {
                        points[current] = points.GetOrCreate(current) + 1;
                        current += (dx, dy);
                    }
                }
            }

            return points.Count(kvp => kvp.Value > 1);
        }
    }
}
