using System;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class RambunctiousRecitationTest
    {
        private readonly RambunctiousRecitation _solver;

        public RambunctiousRecitationTest()
        {
            _solver = new RambunctiousRecitation();
        }

        [Theory]
        [InlineData("0,3,6", 436)]
        [InlineData("1,3,2", 1)]
        [InlineData("2,1,3", 10)]
        [InlineData("1,2,3", 27)]
        [InlineData("2,3,1", 78)]
        [InlineData("3,2,1", 438)]
        [InlineData("3,1,2", 1836)]
        public void Should_solve_example_1(string input, int expected)
        {
            var result = _solver.Solve(input, 2020);

            result.ShouldBe(expected);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = "19,0,5,1,10,13";

            var result = _solver.Solve(input, 2020);

            Console.WriteLine($"RambunctiousRecitation Part 1: {result}");
        }

        [Theory]
        [InlineData("0,3,6", 175594)]
        [InlineData("1,3,2", 2578)]
        [InlineData("2,1,3", 3544142)]
        [InlineData("1,2,3", 261214)]
        [InlineData("2,3,1", 6895259)]
        [InlineData("3,2,1", 18)]
        [InlineData("3,1,2", 362)]
        public void Should_solve_example_2(string input, int expected)
        {
            var result = _solver.Solve(input, 30000000);

            result.ShouldBe(expected);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = "19,0,5,1,10,13";

            var result = _solver.Solve(input, 30000000);

            Console.WriteLine($"RambunctiousRecitation Part 2: {result}");
        }
    }
}
