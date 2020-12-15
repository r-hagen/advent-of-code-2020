using System;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class ShuttleSearchTest
    {
        private readonly ShuttleSearch _solver;

        public ShuttleSearchTest()
        {
            _solver = new ShuttleSearch();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = new[] {
                "939",
                "7,13,x,x,59,x,31,19"
            };

            var result = _solver.Solve1(input);

            result.ShouldBe(295);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = new[] {
                "1000495",
                "19,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,521,x,x,x,x,x,x,x,23,x,x,x,x,x,x,x,x,17,x,x,x,x,x,x,x,x,x,x,x,29,x,523,x,x,x,x,x,37,x,x,x,x,x,x,13"
            };

            var result = _solver.Solve1(input);

            Console.WriteLine($"ShuttleSearch Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = "17,x,13,19";

            var result = _solver.Solve2(input);

            result.ShouldBe(3417ul);
        }

        [Fact]
        public void Should_solve_example_3()
        {
            var input = "67,7,59,61";

            var result = _solver.Solve2(input);

            result.ShouldBe(754018ul);
        }

        [Fact]
        public void Should_solve_example_4()
        {
            var input = "67,x,7,59,61";

            var result = _solver.Solve2(input);

            result.ShouldBe(779210ul);
        }

        [Fact]
        public void Should_solve_example_5()
        {
            var input = "67,7,x,59,61";

            var result = _solver.Solve2(input);

            result.ShouldBe(1261476ul);
        }

        [Fact]
        public void Should_solve_example_6()
        {
            var input = "1789,37,47,1889";

            var result = _solver.Solve2(input);

            result.ShouldBe(1202161486ul);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = "19,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,521,x,x,x,x,x,x,x,23,x,x,x,x,x,x,x,x,17,x,x,x,x,x,x,x,x,x,x,x,29,x,523,x,x,x,x,x,37,x,x,x,x,x,x,13";

            var result = _solver.Solve2(input);

            Console.WriteLine($"ShuttleSearch Part 2: {result}");
        }

        [Fact]
        public void Should_calculate_modular_inverse()
        {
            // https://www.youtube.com/watch?v=zIFehsBHB8o
            _solver.ModularInverse(56, 5).ShouldBe(1);
            _solver.ModularInverse(40, 7).ShouldBe(3);
            _solver.ModularInverse(35, 8).ShouldBe(3);
        }
    }
}
