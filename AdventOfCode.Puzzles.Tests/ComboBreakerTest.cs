using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class ComboBreakerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ComboBreakerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var cardPublicKey = 5764801;
            var doorPublicKey = 17807724;

            var result = ComboBreaker.Solve1(cardPublicKey, doorPublicKey);

            result.ShouldBe("14897079");
        }

        [Fact]
        public void Should_solve_part_1()
        {
            var cardPublicKey = 15335876;
            var doorPublicKey = 15086442;

            var result = ComboBreaker.Solve1(cardPublicKey, doorPublicKey);

            _testOutputHelper.WriteLine($"ComboBreaker Part 1: {result}");
        }
    }
}