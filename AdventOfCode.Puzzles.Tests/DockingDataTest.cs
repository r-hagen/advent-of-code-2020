using System.IO;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class DockingDataTest
    {
        public const string PuzzleFile = "Assets/DockingData.txt";
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly DockingData _solver;

        public DockingDataTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _solver = new DockingData();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[]
            {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0"
            };

            var result = _solver.Solve1(input);

            result.ShouldBe(165ul);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve1(input);

            _testOutputHelper.WriteLine($"DockingData Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new[]
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };

            var result = _solver.Solve2(input);

            result.ShouldBe(208ul);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = _solver.Solve2(input);

            _testOutputHelper.WriteLine($"DockingData Part 2: {result}");
        }
    }
}