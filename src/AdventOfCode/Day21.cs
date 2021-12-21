using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        public int Part1(string[] input)
        {
            // use positions and dice indexed from 0 to make the logic easier
            int die = 0;
            int position1 = input[0].Numbers<int>()[1] - 1;
            int position2 = input[1].Numbers<int>()[1] - 1;

            int rolls = 0;
            int score1 = 0;
            int score2 = 0;
            bool player1 = true;

            while (score1 < 1000 && score2 < 1000)
            {
                int moves = 0;

                for (int i = 0; i < 3; i++)
                {
                    rolls++;
                    moves += die + 1;
                    die = (die + 1) % 100;
                }

                if (player1)
                {
                    position1 = (position1 + moves) % 10;
                    score1 += position1 + 1;
                    player1 = false;
                }
                else
                {
                    position2 = (position2 + moves) % 10;
                    score2 += position2 + 1;
                    player1 = true;
                }
            }

            return Math.Min(score1, score2) * rolls;
        }

        public long Part2(string[] input)
        {
            // use positions indexed from 0 to make the logic easier
            int position1 = input[0].Numbers<int>()[1] - 1;
            int position2 = input[1].Numbers<int>()[1] - 1;

            (long one, long two) = CountWinners(position1, position2);

            return Math.Max(one, two);
        }

        /// <summary>
        /// Count the number of winners in total for the given starting positions of the two players
        /// </summary>
        /// <param name="start1">Player 1 starting position</param>
        /// <param name="start2">Player 2 starting position</param>
        /// <returns>Number of times each player won</returns>
        public static (long one, long two) CountWinners(int start1, int start2)
        {
            int[] dice = { 1, 2, 3 };

            return CountWinnersInternal(start1, 0, start2, 0, true, new Dictionary<(int, int, int, int, bool), (long, long)>());

            (long one, long two) CountWinnersInternal(int position1, int score1, int position2, int score2, bool player1Turn, IDictionary<(int, int, int, int, bool), (long, long)> cache)
            {
                if (score1 >= 21)
                {
                    return (1, 0);
                }

                if (score2 >= 21)
                {
                    return (0, 1);
                }

                long oneWins = 0;
                long twoWins = 0;

                // TODO: Really this only needs to consider the 7 possible totals instead of all 27 possibilities, but then the result needs weighting also
                foreach (int moves in MoreEnumerable.Cartesian(dice, dice, dice, (a,b,c) => a + b + c))
                {
                    (long one, long two) subWinners;

                    if (player1Turn)
                    {
                        int nextPosition = (position1 + moves) % 10;
                        int nextScore = score1 + nextPosition + 1;

                        subWinners = cache.GetOrCreate((nextPosition, nextScore, position2, score2, false),
                                                       () => CountWinnersInternal(nextPosition, nextScore, position2, score2, false, cache));
                    }
                    else
                    {
                        int nextPosition = (position2 + moves) % 10;
                        int nextScore = score2 + nextPosition + 1;

                        subWinners = cache.GetOrCreate((position1, score1, nextPosition, nextScore, true),
                                                       () => CountWinnersInternal(position1, score1, nextPosition, nextScore, true, cache));
                    }

                    oneWins += subWinners.one;
                    twoWins += subWinners.two;
                }
                
                return (oneWins, twoWins);
            }
        }
    }
}
