using System.IO;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class AllergenAssessmentTest
    {
        private const string PuzzleFile = "Assets/AllergenAssessment.txt";
        private readonly ITestOutputHelper _testOutputHelper;

        public AllergenAssessmentTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[]
            {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)"
            };

            var result = AllergenAssessment.Solve1(input);

            result.ShouldBe("5");
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = AllergenAssessment.Solve1(input);

            _testOutputHelper.WriteLine($"AllergenAssessment Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = new[]
            {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)"
            };

            var result = AllergenAssessment.Solve2(input);

            result.ShouldBe("mxmxvkd,sqjhc,fvjkl");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = AllergenAssessment.Solve2(input);

            _testOutputHelper.WriteLine($"AllergenAssessment Part 2: {result}");
        }
    }
}