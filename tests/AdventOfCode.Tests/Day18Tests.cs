using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day18Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day18 solver;

        public Day18Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day18();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day18.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3935;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 18 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("[[9,1],[1,9]]", 129)]
        [InlineData("[[1,2],[[3,4],5]]", 143)]
        [InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
        [InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
        [InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
        [InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
        [InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
        public void Part1_SimpleSampleInput_ProducesCorrectResponse(string input, long expected)
        {
            var result = solver.Part1(new [] { input });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_Addition_ProducesCorrectResponse()
        {
            var input = new[]
            {
                "[[[[4,3],4],4],[7,[[8,4],9]]]",
                "[1,1]"
            };

            var result = solver.Part1(input);

            Assert.Equal(1384, result);
        }

        [Fact]
        public void Part1_ComplexSampleInput_ProducesCorrectResponse()
        {
            var input = new[]
            {
                "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
                "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
                "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                "[7,[5,[[3,8],[1,4]]]]",
                "[[2,[2,2]],[8,[8,1]]]",
                "[2,9]",
                "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                "[[[5,[7,4]],7],1]",
                "[[[[4,2],2],6],[8,7]]",
            };

            var result = solver.Part1(input);

            Assert.Equal(3488, result);
        }

        [Fact]
        public void Part1_FinalSampleInput_ProducesCorrectResponse()
        {
            var input = new[]
            {
                "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
            };

            var result = solver.Part1(input);

            Assert.Equal(4140, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 4669;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 18 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_LotsOfNumbers_ProducesCorrectFinalSum()
        {
            var input = new[]
            {
                "[1,1]",
                "[2,2]",
                "[3,3]",
                "[4,4]",
                "[5,5]",
                "[6,6]"
            };
            
            var node = input.Skip(1).Aggregate(Day18.Parse(input[0]), (current, line) => Day18.Add(current, Day18.Parse(line)));

            StringBuilder sb = new StringBuilder();
            node.Debug(sb);
            Assert.Equal("[[[[5,0],[7,4]],[5,5]],[6,6]]", sb.ToString());
        }

        [Fact]
        public void Add_WhenCalled_AddsAndReducesProperly()
        {
            string leftInput  = "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]";
            string rightInput = "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]";
            var left = Day18.Parse(leftInput);
            var right = Day18.Parse(rightInput);

            var root = Day18.Add(left, right);

            StringBuilder sb = new StringBuilder();
            root.Debug(sb);
            Assert.Equal("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", sb.ToString());
        }
    }
}
