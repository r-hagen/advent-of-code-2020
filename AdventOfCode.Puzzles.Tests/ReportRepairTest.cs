using System;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class ReportRepairTest
    {
        public const string ExampleFile = "Assets/ReportRepair_Example.txt";
        public const string PuzzleFile = "Assets/ReportRepair.txt";

        private readonly ReportRepair _solver;

        public ReportRepairTest()
        {
            _solver = new ReportRepair();
        }

        [Fact]
        public void Should_parse_example_input()
        {
            var result = _solver.ParseInput(ExampleFile);

            result.Length.ShouldBe(6);
            result.ShouldBe(new int[] { 1721, 979, 366, 299, 675, 1456 });
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var result = _solver.Solve1(ExampleFile);

            result.ShouldNotBeNull();
            result.ShouldBe(514579);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var result = _solver.Solve1(PuzzleFile);

            result.ShouldNotBeNull();
            Console.WriteLine($"ReportRepair Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var result = _solver.Solve2(ExampleFile);

            result.ShouldNotBeNull();
            result.ShouldBe(241861950);
        }

        [Fact]
        public void Solve_puzzle_2()
        {
            var result = _solver.Solve2(PuzzleFile);

            result.ShouldNotBeNull();
            Console.WriteLine($"ReportRepair Part 2: {result}");
        }
    }
}
