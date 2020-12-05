using System;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class PasswordPhilosophyTest
    {
        public const string ExampleFile = "Assets/PasswordPhilosophy_Example.txt";
        public const string PuzzleFile = "Assets/PasswordPhilosophy.txt";

        private readonly PasswordPhilosophy _solver;

        public PasswordPhilosophyTest()
        {
            _solver = new PasswordPhilosophy();
        }

        [Fact]
        public void Should_parse_input()
        {
            var result = _solver.ParseInput(ExampleFile);

            result.Length.ShouldBe(3);

            var first = result.First();
            first.Lowest.ShouldBe(1);
            first.Highest.ShouldBe(3);
            first.Letter.ShouldBe('a');
            first.Password.ShouldBe("abcde");
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var result = _solver.Solve1(ExampleFile);

            result.ShouldBe(2);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var result = _solver.Solve1(PuzzleFile);

            Console.WriteLine($"PasswordPhilosophy Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var result = _solver.Solve2(ExampleFile);

            result.ShouldBe(1);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var result = _solver.Solve2(PuzzleFile);

            Console.WriteLine($"PasswordPhilosophy Part 2: {result}");
        }
    }
}
