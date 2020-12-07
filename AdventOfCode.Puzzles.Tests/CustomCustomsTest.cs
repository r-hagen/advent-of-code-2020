using System;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class CustomCustomsTest
    {
        public const string ExampleFile = "Assets/CustomCustoms_Example.txt";
        public const string PuzzleFile = "Assets/CustomCustoms.txt";

        private readonly CustomCustoms _solver;

        public CustomCustomsTest()
        {
            _solver = new CustomCustoms();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var result = _solver.Solve1(ExampleFile);

            result.ShouldBe(11);
        }

        [Fact]
        public void Should_solve_puzzle()
        {
            var result = _solver.Solve1(PuzzleFile);

            Console.WriteLine($"CustomCustoms Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var result = _solver.Solve2(ExampleFile);

            result.ShouldBe(6);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var result = _solver.Solve2(PuzzleFile);

            Console.WriteLine($"CustomCustoms Part 2: {result}");
        }
    }
}
