using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class CrabCupsTest
    {
        public CrabCupsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        [Fact]
        public void Should_solve_example_1()
        {
            const string input = "389125467";

            var result = CrabCups.Solve1(input, 10);

            result.ShouldBe("92658374");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            const string input = "389125467";

            var result = CrabCups.Solve2(input, 10_000_000);

            result.ShouldBe("149245887792");
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            const string input = "157623984";

            var result = CrabCups.Solve1(input, 100);

            _testOutputHelper.WriteLine($"CrabCups Part 1: {result}");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            const string input = "157623984";

            var result = CrabCups.Solve2(input, 10_000_000);

            _testOutputHelper.WriteLine($"CrabCups Part 2: {result}");
        }
    }
}
