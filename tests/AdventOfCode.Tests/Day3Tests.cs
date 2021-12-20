using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day3Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day3 solver;

        public Day3Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day3();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day3.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 749376;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 3 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 2372923;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 3 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_Sample()
        {
            string[] input =
            {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010"
            };

            var result = solver.Part2(input);

            Assert.Equal(230, result);
        }

    }
}
