using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 13
    /// </summary>
    public class Day13
    {
        public int Part1(string[] input)
        {
            (HashSet<Point2D> points, List<(bool vertical, int location)> instructions) = ParseInput(input);

            // apply first fold
            (bool vertical, int location) = instructions.First();

            points = vertical switch
            {
                true => FoldVertical(points, location),
                false => FoldHorizontal(points, location)
            };

            return points.Count;
        }

        public string Part2(string[] input)
        {
            (HashSet<Point2D> points, List<(bool vertical, int location)> instructions) = ParseInput(input);

            // apply all rules
            foreach ((bool vertical, int location) in instructions)
            {
                points = vertical switch
                {
                    true => FoldVertical(points, location),
                    false => FoldHorizontal(points, location)
                };
            }

            StringBuilder result = new StringBuilder();

            // print the answer
            int maxY = points.Select(p => p.Y).Max() + 1;
            int maxX = points.Select(p => p.X).Max() + 1;

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    result.Append(points.Contains((x, y)) ? '#' : ' ');
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse the input into a set of points and instructions to follow
        /// </summary>
        /// <param name="input">Input lines</param>
        /// <returns>Points and fold instructions</returns>
        private static (HashSet<Point2D> points, List<(bool vertical, int location)> instructions) ParseInput(string[] input)
        {
            var points = new HashSet<Point2D>();
            var instructions = new List<(bool vertical, int location)>();
            var parsingPoints = true;

            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingPoints = false;
                    continue;
                }

                if (parsingPoints)
                {
                    var numbers = line.Numbers<int>();
                    points.Add((numbers[0], numbers[1]));
                }
                else
                {
                    instructions.Add((line.Contains("x"), line.Numbers<int>().First()));
                }
            }

            return (points, instructions);
        }

        /// <summary>
        /// Fold all the points from the right of the given line over to the left
        /// </summary>
        /// <param name="points">Points</param>
        /// <param name="x">Vertical fold location</param>
        /// <returns>New points</returns>
        private static HashSet<Point2D> FoldVertical(HashSet<Point2D> points, int x)
        {
            var folded = new HashSet<Point2D>();

            foreach (Point2D point in points)
            {
                if (point.X < x)
                {
                    folded.Add(point);
                    continue;
                }

                var dx = point.X - x; // distance from point to fold
                int newX = x - dx; // distance from fold to new point

                folded.Add(new Point2D(newX, point.Y));
            }

            return folded;
        }

        /// <summary>
        /// Fold all the points from below the given line up above it
        /// </summary>
        /// <param name="points">Points</param>
        /// <param name="y">Horizontal fold location</param>
        /// <returns>New points</returns>
        private static HashSet<Point2D> FoldHorizontal(HashSet<Point2D> points, int y)
        {
            var folded = new HashSet<Point2D>();

            foreach (Point2D point in points)
            {
                if (point.Y < y)
                {
                    folded.Add(point);
                    continue;
                }

                var dy = point.Y - y; // distance from point to fold
                int newY = y - dy; // distance from fold to new point

                folded.Add(new Point2D(point.X, newY));
            }

            return folded;
        }
    }
}
