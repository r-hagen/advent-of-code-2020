using System;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class TobogganTrajectoryTest
    {
        public const string ExampleFile = "Assets/TobogganTrajectory_Example.txt";
        public const string PuzzleFile = "Assets/TobogganTrajectory.txt";

        private readonly TobogganTrajectory _solver;

        public TobogganTrajectoryTest()
        {
            _solver = new TobogganTrajectory();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var result = _solver.Solve1(ExampleFile, 3, 1);

            result.ShouldBe(7);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var result = _solver.Solve1(PuzzleFile, 3, 1);

            Console.WriteLine($"TobogganTrajectory Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var result =
                _solver.Solve1(ExampleFile, 1, 1) *
                _solver.Solve1(ExampleFile, 3, 1) *
                _solver.Solve1(ExampleFile, 5, 1) *
                _solver.Solve1(ExampleFile, 7, 1) *
                _solver.Solve1(ExampleFile, 1, 2);

            result.ShouldBe(336);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var result =
                (ulong)_solver.Solve1(PuzzleFile, 1, 1) *
                (ulong)_solver.Solve1(PuzzleFile, 3, 1) *
                (ulong)_solver.Solve1(PuzzleFile, 5, 1) *
                (ulong)_solver.Solve1(PuzzleFile, 7, 1) *
                (ulong)_solver.Solve1(PuzzleFile, 1, 2);

            Console.WriteLine($"TobogganTrajectory Part 2: {result}");
        }
    }
}
