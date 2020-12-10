using System;
using System.IO;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class AdapterArrayTest
    {
        public const string PuzzleFile = "Assets/AdapterArray.txt";

        private readonly AdapterArray _solver;

        public AdapterArrayTest()
        {
            _solver = new AdapterArray();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 };

            var result = _solver.Solve1(input);

            result.ShouldBe(7 * 5);
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new[] { 28 ,33 ,18 ,42 ,31 ,14 ,46 ,20 ,48 ,47 ,24
                ,23 ,49 ,45 ,19 ,38 ,39 ,11 ,1 ,32 ,25 ,35 ,8 ,17 ,7 ,9 ,4 ,2
                ,34 ,10 ,3
            };

            var result = _solver.Solve1(input);

            result.ShouldBe(22 * 10);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile)
                .Select(x => int.Parse(x))
                .ToArray();

            var result = _solver.Solve1(input);

            Console.WriteLine($"AdapterArray Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_3()
        {
            var input = new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 };

            var result = _solver.Solve2(input);

            result.ShouldBe(8ul);
        }

        [Fact]
        public void Should_solve_example_4()
        {
            var input = new[] { 28 ,33 ,18 ,42 ,31 ,14 ,46 ,20 ,48 ,47 ,24
                ,23 ,49 ,45 ,19 ,38 ,39 ,11 ,1 ,32 ,25 ,35 ,8 ,17 ,7 ,9 ,4 ,2
                ,34 ,10 ,3
            };

            var result = _solver.Solve2(input);

            result.ShouldBe(19208ul);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile)
                .Select(x => int.Parse(x))
                .ToArray();

            var result = _solver.Solve2(input);

            Console.WriteLine($"AdapterArray Part 2: {result}");
        }
    }
}
