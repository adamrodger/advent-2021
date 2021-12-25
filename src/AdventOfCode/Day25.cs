using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 25
    /// </summary>
    public class Day25
    {
        public int Part1(string[] input)
        {
            var east = new HashSet<Point2D>();
            var south = new HashSet<Point2D>();
            int width = input[0].Length;
            int height = input.Length;

            foreach ((int y, string line) in input.Enumerate())
            {
                foreach ((int x, char c) in line.Enumerate())
                {
                    if (c == '>')
                    {
                        east.Add((x, y));
                    }
                    else if (c == 'v')
                    {
                        south.Add((x, y));
                    }
                }
            }

            bool moved = true;
            int steps = 0;

            while (moved)
            {
                steps++;

                (moved, east, south) = Step(east, south, width, height);
            }

            return steps;
        }

        /// <summary>
        /// Move all east-facing items then all south-facing ones, and indicate if any actually moved
        /// </summary>
        private static (bool moved, HashSet<Point2D> east, HashSet<Point2D> south) Step(HashSet<Point2D> east, HashSet<Point2D> south, int width, int height)
        {
            var moved = false;
            var nextEast = new HashSet<Point2D>();
            var nextSouth = new HashSet<Point2D>();

            foreach (Point2D point in east)
            {
                Point2D right = ((point.X + 1) % width, point.Y);

                if (east.Contains(right) || south.Contains(right))
                {
                    // can't move
                    nextEast.Add(point);
                }
                else
                {
                    moved = true;
                    nextEast.Add(right);
                }
            }

            foreach (Point2D point in south)
            {
                Point2D below = (point.X, (point.Y + 1) % height);

                // use nextEast, not previous east
                if (nextEast.Contains(below) || south.Contains(below))
                {
                    // can't move
                    nextSouth.Add(point);
                }
                else
                {
                    moved = true;
                    nextSouth.Add(below);
                }
            }

            return (moved, nextEast, nextSouth);
        }
    }
}
