using System;
using System.IO;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class HandheldHaltingTest
    {
        public const string PuzzleFile = "Assets/HandheldHalting.txt";

        private readonly HandheldHalting _solver;

        public HandheldHaltingTest()
        {
            _solver = new HandheldHalting();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[] {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            var result = _solver.Solve1(input);

            result.ShouldBe(5);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve1(input);

            Console.WriteLine($"HandheldHalting Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new[] {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            var result = _solver.Solve2(input);

            result.ShouldBe(8);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve2(input);

            Console.WriteLine($"HandheldHalting Part 2: {result}");
        }
    }
}
