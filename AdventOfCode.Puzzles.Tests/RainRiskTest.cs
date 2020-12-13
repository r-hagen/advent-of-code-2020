using System;
using System.IO;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class RainRiskTest
    {
        public const string PuzzleFile = "Assets/RainRisk.txt";

        private readonly RainRisk _solver;

        public RainRiskTest()
        {
            _solver = new RainRisk();
        }

        [Fact]
        public void Should_parse_input()
        {
            var input = new[] { "F10", "N3", "F7", "R90", "F11" };

            var result = _solver.ParseInput(input);

            result.First().ShouldBe(('F', 10));
            result.Last().ShouldBe(('F', 11));
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[] { "F10", "N3", "F7", "R90", "F11" };

            var result = _solver.Solve1(input);

            result.ShouldBe(25);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve1(input);

            Console.WriteLine($"RainRisk Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new[] { "F10", "N3", "F7", "R90", "F11" };

            var result = _solver.Solve2(input);

            result.ShouldBe(286);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve2(input);

            Console.WriteLine($"RainRisk Part 2: {result}");
        }
    }
}
