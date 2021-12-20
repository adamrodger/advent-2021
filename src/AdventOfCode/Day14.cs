using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 14
    /// </summary>
    public class Day14
    {
        public long Part1(string[] input)
        {
            return Calculate(input, 10);
        }

        public long Part2(string[] input)
        {
            return Calculate(input, 40);
        }

        /// <summary>
        /// Calculate the difference between the most and least common elements after
        /// a number of rounds of transformation
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="rounds">Number of rounds to perform</param>
        /// <returns>Difference between most and least common element</returns>
        private static long Calculate(string[] input, int rounds)
        {
            (string start, Dictionary<string, char> instructions) = ParseInput(input);

            // load initial state
            Dictionary<string, long> pairs = CreateInitialState(start);

            // perform transformation
            for (int i = 0; i < rounds; i++)
            {
                pairs = Transform(pairs, instructions);
            }

            // count the number of each element
            Dictionary<char, long> total = new Dictionary<char, long>
            {
                [start[^1]] = 1 // the final char won't be counted below so initialise here
            };

            foreach ((string key, long value) in pairs)
            {
                char c = key[0];
                total[c] = total.GetValueOrDefault(c) + value;
            }

            // calculate the difference
            var biggest = total.Values.Max();
            var smallest = total.Values.Min();

            return biggest - smallest;
        }

        /// <summary>
        /// Parse the input into a starting state and a set of transformation instructions
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Start and instructions</returns>
        private static (string start, Dictionary<string, char> instructions) ParseInput(string[] input)
        {
            var start = input[0];
            var instructions = new Dictionary<string, char>();

            foreach (string line in input.Skip(2))
            {
                var parts = line.Trim().Split(" -> ");
                instructions[parts[0]] = parts[1][0];
            }

            return (start, instructions);
        }

        /// <summary>
        /// Create the initial state before any transformations have been applied
        /// </summary>
        /// <param name="start">Starting state</param>
        /// <returns>Lookup of frequency of each pair in the initial state</returns>
        private static Dictionary<string, long> CreateInitialState(string start)
        {
            Dictionary<string, long> pairs = new Dictionary<string, long>();

            for (int i = 0; i < start.Length - 1; i++)
            {
                string current = start[i..(i + 2)];
                pairs[current] = pairs.GetValueOrDefault(current) + 1;
            }

            return pairs;
        }

        /// <summary>
        /// Transform the input state into an output state following the given instructions
        /// </summary>
        /// <param name="input">Input state</param>
        /// <param name="instructions">Transformation instructions</param>
        /// <returns>Output state</returns>
        private static Dictionary<string, long> Transform(Dictionary<string, long> input, Dictionary<string, char> instructions)
        {
            var output = new Dictionary<string, long>();

            foreach ((string key, long value) in input)
            {
                char replace = instructions[key];

                // effectively each input doubles itself
                string left = $"{key[0]}{replace}";
                output[left] = output.GetValueOrDefault(left) + value;

                string right = $"{replace}{key[1]}";
                output[right] = output.GetValueOrDefault(right) + value;
            }

            return output;
        }
    }
}
