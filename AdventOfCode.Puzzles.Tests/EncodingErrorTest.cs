using System;
using System.IO;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class EncodingErrorTest
    {
        public const string PuzzleFile = "Assets/EncodingError.txt";

        private readonly EncodingError _solver;

        public EncodingErrorTest()
        {
            _solver = new EncodingError();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new ulong[] { 35 ,20 ,15 ,25 ,47 ,40 ,62 ,55 ,65 ,95
                ,102 ,117 ,150 ,182 ,127 ,219 ,299 ,277 ,309 ,576 };

            var result = _solver.Solve1(input, 5);

            result.ShouldBe(127u);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile).Select(l => ulong.Parse(l)).ToArray();

            var result = _solver.Solve1(input, 25);

            Console.WriteLine($"EncodingError Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new ulong[] { 35 ,20 ,15 ,25 ,47 ,40 ,62 ,55 ,65 ,95
                ,102 ,117 ,150 ,182 ,127 ,219 ,299 ,277 ,309 ,576 };

            var result = _solver.Solve2(input, 5);

            result.ShouldBe(62u);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile).Select(l => ulong.Parse(l)).ToArray();

            var result = _solver.Solve2(input, 25);

            Console.WriteLine($"EncodingError Part 2: {result}");
        }
    }
}
