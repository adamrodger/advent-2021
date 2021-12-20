using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 1
    /// </summary>
    public class Day1
    {
        public int Part1(string[] input)
        {
            var numbers = input.Select(int.Parse).ToArray();

            return numbers.Zip(numbers.Skip(1)).Count(pair => pair.Second > pair.First);
        }

        public int Part2(string[] input)
        {
            var numbers = input.Select(int.Parse).ToArray();
            var windows = numbers.Window(3).Select(w => w.Sum()).ToArray();

            return windows.Zip(windows.Skip(1)).Count(pair => pair.Second > pair.First);
        }
    }
}
