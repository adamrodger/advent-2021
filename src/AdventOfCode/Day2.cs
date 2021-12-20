using System;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 2
    /// </summary>
    public class Day2
    {
        public int Part1(string[] input)
        {
            int position = 0;
            int depth = 0;

            foreach (string line in input)
            {
                var parts = line.Split(' ');
                int value = int.Parse(parts[1]);

                (position, depth) = parts[0] switch
                {
                    "forward" => (position + value, depth),
                    "down"    => (position,         depth + value),
                    "up"      => (position,         depth - value),
                    _ => throw new ArgumentOutOfRangeException(nameof(parts), parts[0], "Unsupported movement")
                };
            }

            return position * depth;
        }

        public int Part2(string[] input)
        {
            int position = 0;
            int depth = 0;
            int aim = 0;

            foreach (string line in input)
            {
                var parts = line.Split(' ');
                int value = int.Parse(parts[1]);

                (position, depth, aim) = parts[0] switch
                {
                    "forward" => (position + value, depth + (aim * value), aim),
                    "down"    => (position,         depth,                 aim + value),
                    "up"      => (position,         depth,                 aim - value),
                    _ => throw new ArgumentOutOfRangeException(nameof(parts), parts[0], "Unsupported movement")
                };
            }

            return position * depth;
        }
    }
}
