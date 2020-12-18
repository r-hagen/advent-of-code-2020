using System;
using System.IO;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class TicketTranslationTest
    {
        private const string PuzzleFile = "Assets/TicketTranslation.txt";

        private readonly TicketTranslation _solver;

        public TicketTranslationTest()
        {
            _solver = new TicketTranslation();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[]
            {
                "class: 1-3 or 5-7",
                "row: 6-11 or 33-44",
                "seat: 13-40 or 45-50",
                "",
                "your ticket:",
                "7,1,14",
                "",
                "nearby tickets:",
                "7,3,47",
                "40,4,50",
                "55,2,20",
                "38,6,12",
            };

            var result = _solver.Solve(input);

            result.ShouldBe(71);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve(input);

            Console.WriteLine($"TicketTranslation Part 1: {result}");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve2(input);

            Console.WriteLine($"TicketTranslation Part 2: {result}");
        }
    }
}
