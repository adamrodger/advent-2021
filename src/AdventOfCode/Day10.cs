using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 10
    /// </summary>
    public class Day10
    {
        public int Part1(string[] input)
        {
            return input.Select(CheckLine).Sum();
        }

        public long Part2(string[] input)
        {
            string[] incomplete = input.Where(i => CheckLine(i) == 0).ToArray();

            List<long> scores = incomplete.Select(CompleteLine).ToList();
            scores.Sort();

            return scores[scores.Count / 2];
        }

        /// <summary>
        /// Checks a line and returns score indicating if it was incomplete
        /// </summary>
        /// <param name="line">Line to check</param>
        /// <returns>Syntax error score, or 0 if there was no syntax error</returns>
        private static int CheckLine(string line)
        {
            var pairs = new Dictionary<char, char>
            {
                ['('] = ')',
                ['['] = ']',
                ['{'] = '}',
                ['<'] = '>'
            };

            Stack<char> open = new Stack<char>();

            foreach (char c in line)
            {
                if (pairs.ContainsKey(c))
                {
                    open.Push(c);
                    continue;
                }

                char expected = open.Pop();

                if (c != pairs[expected])
                {
                    return c switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137,
                        _ => throw new ArgumentOutOfRangeException(nameof(c), c, "Invalid closing char")
                    };
                }
            }

            // either valid or incomplete, but not corrupt
            return 0;
        }

        /// <summary>
        /// Complete the given line, which can be assumed to be incomplete
        /// </summary>
        /// <param name="line">Line</param>
        /// <returns>Cost for completing the line</returns>
        private static long CompleteLine(string line)
        {
            Stack<char> open = new Stack<char>();

            foreach (char c in line)
            {
                if (c is '(' or '[' or '{' or '<')
                {
                    open.Push(c);
                    continue;
                }

                // assume it's closed properly
                open.Pop();
            }

            // we'll now be left with the unclosed opening chars, so loop through and score them
            long score = 0;

            while (open.Any())
            {
                char c = open.Pop();

                score *= 5;

                score += c switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                    _ => throw new ArgumentOutOfRangeException(nameof(c), c, "Invalid closing char")
                };
            }

            return score;
        }
    }
}
