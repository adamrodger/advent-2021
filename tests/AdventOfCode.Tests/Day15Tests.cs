using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day15Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day15 solver;

        public Day15Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day15();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day15.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new[]
            {
                "1163751742",
                "1381373672",
                "2136511328",
                "3694931569",
                "7463417111",
                "1319128137",
                "1359912421",
                "3125421639",
                "1293138521",
                "2311944581"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 40;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 523;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 15 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 315;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 2876;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 15 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
