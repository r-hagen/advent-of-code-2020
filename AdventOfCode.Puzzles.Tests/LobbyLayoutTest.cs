using System.IO;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class LobbyLayoutTest
    {
        public LobbyLayoutTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        private const string PuzzleFile = "Assets/LobbyLayout.txt";
        private const string ExampleFile = "Assets/LobbyLayoutExample.txt";

        [Fact]
        public void Should_solve_example_1()
        {
            var input = File.ReadAllLines(ExampleFile);

            var result = LobbyLayout.Solve1(input);

            result.ShouldBe("10");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = File.ReadAllLines(ExampleFile);

            var result = LobbyLayout.Solve2(input, 100);

            result.ShouldBe("2208");
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = LobbyLayout.Solve1(input);

            _testOutputHelper.WriteLine($"LobbyLayout Part 1: {result}");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = LobbyLayout.Solve2(input, 100);

            _testOutputHelper.WriteLine($"LobbyLayout Part 2: {result}");
        }
    }
}
