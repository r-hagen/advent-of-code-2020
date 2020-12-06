using System;
using System.IO;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles.Tests
{
    public class BinaryBoardingTest
    {
        public const string PuzzleFile = "Assets/BinaryBoarding.txt";

        private readonly BinaryBoarding _solver;

        public BinaryBoardingTest()
        {
            _solver = new BinaryBoarding();
        }

        [Theory]
        [InlineData("FBFBBFFRLR", 44, 5, 357)]
        [InlineData("BFFFBBFRRR", 70, 7, 567)]
        [InlineData("FFFBBBFRRR", 14, 7, 119)]
        [InlineData("BBFFBBFRLL", 102, 4, 820)]
        public void Should_decode_boarding_pass(string pass, int row, int col, int id)
        {
            var (r, c, i) = _solver.Decode(pass);

            r.ShouldBe(row);
            c.ShouldBe(col);
            i.ShouldBe(id);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var maxId = 0;

            var passes = File.ReadAllLines(PuzzleFile);
            foreach (var p in passes)
            {
                var (_, _, id) = _solver.Decode(p);

                if (id > maxId)
                    maxId = id;
            }

            Console.WriteLine($"BinaryBoarding Part 1: {maxId}");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var ids = new List<int>();

            var passes = File.ReadAllLines(PuzzleFile);
            foreach (var p in passes)
            {
                var (_, _, id) = _solver.Decode(p);
                ids.Add(id);
            }

            var allIds = Enumerable.Range(ids.Min(), ids.Max());
            var missingIds = allIds.Except(ids);

            var solution = missingIds.Single(id => ids.Contains(id - 1) && ids.Contains(id + 1));

            Console.WriteLine($"BinaryBoarding Part 2: {solution}");
        }
    }
}
