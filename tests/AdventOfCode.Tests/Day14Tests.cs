using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day14 solver;

        public Day14Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day14();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day14.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "NNCB",
                "",
                "CH -> B",
                "HH -> N",
                "CB -> H",
                "NH -> C",
                "HB -> C",
                "HC -> B",
                "HN -> C",
                "NN -> C",
                "BH -> H",
                "NC -> B",
                "NB -> B",
                "BN -> B",
                "BB -> N",
                "BC -> B",
                "CC -> N",
                "CN -> C"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 1588;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3230;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 14 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 2188189693529;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 3542388214529;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 14 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
