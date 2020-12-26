using System;
using System.IO;
using System.Linq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class CrabCombatTest
    {
        private const string PuzzleFile = "Assets/CrabCombat.txt";
        private readonly ITestOutputHelper _testOutputHelper;

        public CrabCombatTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input =
                @"Player 1:
                9
                2
                6
                3
                1

                Player 2:
                5
                8
                4
                7
                10"
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();

            var result = CrabCombat.Solve1(input);

            result.ShouldBe("306");
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = CrabCombat.Solve1(input);

            _testOutputHelper.WriteLine($"CrabCombat Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input =
                @"Player 1:
                9
                2
                6
                3
                1

                Player 2:
                5
                8
                4
                7
                10"
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();

            var result = CrabCombat.Solve2(input);

            result.ShouldBe("291");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = CrabCombat.Solve2(input);

            _testOutputHelper.WriteLine($"CrabCombat Part 2: {result}");
        }
    }
}