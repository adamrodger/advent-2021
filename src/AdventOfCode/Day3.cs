using System;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 3
    /// </summary>
    public class Day3
    {
        public int Part1(string[] input)
        {
            int length = input[0].Length;
            int gamma = 0;
            int epsilon = 0;

            for (int i = 0; i < length; i++)
            {
                int shift = length - i - 1;
                var ones = input.Count(line => line[i] == '1');

                if (ones > input.Length / 2)
                {
                    gamma += 1 << shift;
                }
                else
                {
                    epsilon += 1 << shift;
                }
            }

            int result = gamma * epsilon;
            return result;
        }

        public int Part2(string[] input)
        {
            var oxygenCandidates = input.ToList();
            var co2Candidates = input.ToList();

            // oxygen keeps the candidates matching the most common bit in the current position (or 1 on tie break)
            for (int i = 0; i < input[0].Length && oxygenCandidates.Count > 1; i++)
            {
                var ones = oxygenCandidates.Count(line => line[i] == '1');

                if (ones >= (int)Math.Ceiling((double)oxygenCandidates.Count / 2))
                {
                    oxygenCandidates.RemoveAll(o => o[i] == '0');
                }
                else
                {
                    oxygenCandidates.RemoveAll(o => o[i] == '1');
                }
            }

            // co2 keeps the candidates matching the least common bit in the current position (or 0 on tie break)
            for (int i = 0; i < input[0].Length && co2Candidates.Count > 1; i++)
            {
                var ones = co2Candidates.Count(line => line[i] == '1');

                if (ones >= (int)Math.Ceiling((double)co2Candidates.Count / 2))
                {
                    co2Candidates.RemoveAll(o => o[i] == '1');
                }
                else
                {
                    co2Candidates.RemoveAll(o => o[i] == '0');
                }
            }

            int result = Convert.ToInt32(oxygenCandidates.First(), 2) * Convert.ToInt32(co2Candidates.First(), 2);
            return result;
        }
    }
}
