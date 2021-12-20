﻿using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day13Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day13 solver;

        public Day13Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day13();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day13.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 675;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 13 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = string.Join(Environment.NewLine,
                                       "#  # #### #  # #  # #### ####   ## ####",
                                       "#  #    # # #  #  # #    #       #    #",
                                       "####   #  ##   #### ###  ###     #   # ",
                                       "#  #  #   # #  #  # #    #       #  #  ",
                                       "#  # #    # #  #  # #    #    #  # #   ",
                                       "#  # #### #  # #  # #    ####  ##  ####");

            var result = solver.Part2(GetRealInput()).Trim();
            output.WriteLine($"Day 13 - Part 2");
            output.WriteLine("");
            output.WriteLine(result);

            Assert.Equal(expected, result);
        }
    }
}
