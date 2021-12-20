using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day9Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day9 solver;

        public Day9Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day9();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day9.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678",
            };
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 564;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 9 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 1134;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 1038240;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 9 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
