using System;
using System.IO;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class OperationOrderTest
    {
        private const string PuzzleFile = "Assets/OperationOrder.txt";
        private readonly OperationOrder _solver;

        public OperationOrderTest()
        {
            _solver = new OperationOrder();
        }

        [Theory]
        [InlineData("1 + 2", 3)]
        [InlineData("2 * 3", 6)]
        [InlineData("2 * 3 + 4", 10)]
        [InlineData("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Should_solve_example_1(string expression, ulong result)
        {
            _solver.Solve1(expression).ShouldBe(result);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve1(input);

            Console.WriteLine($"OperationOrder Part 1: {result}");
        }

        [Theory]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 1 + 3", 8)]
        [InlineData("2 * 3 + (4 * 5)", 46)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Should_solve_example_2(string expression, ulong result)
        {
            _solver.Solve2(expression).ShouldBe(result);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve2(input);

            Console.WriteLine($"OperationOrder Part 2: {result}");
        }
    }
}
