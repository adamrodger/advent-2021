using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 4
    /// </summary>
    public class Day4
    {
        public int Part1(string[] input)
        {
            int[] calls = input[0].Numbers<int>();

            ICollection<Board> boards = ParseBoards(input.Skip(1));

            foreach (int call in calls)
            {
                foreach (Board board in boards)
                {
                    board.MarkNumber(call);

                    if (board.IsWinner())
                    {
                        return board.RemainingScore() * call;
                    }
                }
            }

            throw new InvalidOperationException("Nobody won");
        }

        public int Part2(string[] input)
        {
            int[] calls = input[0].Numbers<int>();

            ICollection<Board> boards = ParseBoards(input.Skip(1));
            HashSet<Board> remaining = new HashSet<Board>(boards);

            foreach (int call in calls)
            {
                foreach (Board board in boards.Where(b => remaining.Contains(b)))
                {
                    board.MarkNumber(call);

                    if (!board.IsWinner())
                    {
                        continue;
                    }

                    remaining.Remove(board);

                    if (!remaining.Any())
                    {
                        return board.RemainingScore() * call;    
                    }
                }
            }

            throw new InvalidOperationException("No board was last...?");
        }
        
        /// <summary>
        /// Parse the input to a collection of bingo boards
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Parsed bingo boards</returns>
        private static ICollection<Board> ParseBoards(IEnumerable<string> input)
        {
            var boards = new List<Board>();
            Board current = null;

            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    current = new Board();
                    boards.Add(current);
                    continue;
                }

                int[] row = line.Numbers<int>();
                current.Numbers.Add(row);
            }

            return boards;
        }
    }

    /// <summary>
    /// Bingo board
    /// </summary>
    public class Board
    {
        private const int Marked = int.MinValue;
        
        /// <summary>
        /// Board numbers, by row then column
        /// </summary>
        public IList<int[]> Numbers { get; } = new List<int[]>();

        /// <summary>
        /// Mark the given number on this board
        /// </summary>
        /// <param name="n">Number to mark</param>
        public void MarkNumber(int n)
        {
            foreach (int[] row in this.Numbers)
            {
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == n)
                    {
                        row[x] = Marked;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Check if this board is a winner - i.e. it has either a full row or full column marked
        /// </summary>
        /// <returns>Is this board a winner?</returns>
        public bool IsWinner()
        {
            // check rows
            if (this.Numbers.Any(row => row.All(n => n == Marked)))
            {
                return true;
            }
            
            // check columns
            for (int x = 0; x < this.Numbers[0].Length; x++)
            {
                if (this.Numbers.All(row => row[x] == Marked))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the sum of all unmarked numbers
        /// </summary>
        /// <returns>Unmarked numbers sum</returns>
        public int RemainingScore()
        {
            return this.Numbers.SelectMany(row => row).Where(n => n != Marked).Sum();
        }
    }
}
