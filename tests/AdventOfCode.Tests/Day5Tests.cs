using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day5Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day5 solver;

        public Day5Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day5();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day5.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2",
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 5;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3990;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 5 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 12;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 21305;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 5 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
