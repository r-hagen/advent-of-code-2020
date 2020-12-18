using System;
using System.IO;
using Xunit;
using Shouldly;
using static AdventOfCode.Puzzles.ConwayCubes;

namespace AdventOfCode.Puzzles.Tests
{
    public class ConwayCubesTest
    {
        private const string PuzzleFile = "Assets/ConwayCubes.txt";
        private readonly ConwayCubes _solver;

        private readonly string[] Example = new[] {
                ".#.",
                "..#",
                "###"
            };

        public ConwayCubesTest()
        {
            _solver = new ConwayCubes();
        }

        private Cube makeCube(int x, int y, int z = 0, int w = 0)
        {
            return new Cube(x, y, z, w);
        }

        [Fact]
        public void CubeEquality_TwoCubes_ShouldEqual()
        {
            var cubeA = makeCube(1, 2);
            var cubeB = makeCube(1, 2);

            cubeA.ShouldBe(cubeB);
        }

        [Fact]
        public void Should_parse_active_cubes()
        {
            var result = _solver.parseInput(Example);

            result.ActiveCount.ShouldBe(5);
            result.IsActive(0, 0, 0, 0).ShouldBeFalse();
            result.IsActive(1, 0, 0, 0).ShouldBeTrue();
            result.IsActive(2, 1, 0, 0).ShouldBeTrue();
            result.IsActive(0, 2, 0, 0).ShouldBeTrue();
            result.IsActive(1, 2, 0, 0).ShouldBeTrue();
            result.IsActive(2, 2, 0, 0).ShouldBeTrue();

            Console.WriteLine(result);
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[] {
                ".#.",
                "..#",
                "###"
            };

            var result = _solver.Solve(input, 6);

            result.ShouldBe(112);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve(input, 6);

            Console.WriteLine($"ConwayCubes Part 1: {result}");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve(input, 6);

            Console.WriteLine($"ConwayCubes Part 2: {result}");
        }
    }
}
