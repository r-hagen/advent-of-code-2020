using System;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class PassportProcessingTest
    {
        public const string ExampleFile = "Assets/PassportProcessing_Example.txt";
        public const string ValidFile = "Assets/PassportProcessing_Example_Valid.txt";
        public const string PuzzleFile = "Assets/PassportProcessing.txt";

        private readonly PassportProcessing _solver;

        public PassportProcessingTest()
        {
            _solver = new PassportProcessing();
        }

        [Fact]
        public void Should_parse_passport_fields()
        {
            var passports = _solver.ParseInput(ExampleFile);

            passports.Count().ShouldBe(4);

            passports[0].Keys.Count().ShouldBe(8);
            passports[1].Keys.Count().ShouldBe(7);
            passports[2].Keys.Count().ShouldBe(7);
            passports[3].Keys.Count().ShouldBe(6);
        }

        [Fact]
        public void Should_parse_passport_field_keys()
        {
            var passports = _solver.ParseInput(ExampleFile);

            var passportKeys = passports.First().Keys;
            var passportFields = _solver.PassportFields();

            foreach (var fn in passportFields)
                passportKeys.ShouldContain(fn);
        }

        [Fact]
        public void Should_solve_example()
        {
            var result = _solver.Solve1(ExampleFile);

            result.ShouldBe(2);
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var result = _solver.Solve1(PuzzleFile);

            Console.WriteLine($"PassportProcessing Part 1: {result}");
        }

        [Fact]
        public void Should_be_valid_year()
        {
            _solver.IsValidYear("2020", 1900, 2030).ShouldBeTrue();
            _solver.IsValidYear("2020", 1900, 2020).ShouldBeTrue();
            _solver.IsValidYear("2020", 1900, 2010).ShouldBeFalse();
        }

        [Fact]
        public void Should_all_be_valid_2()
        {
            var result = _solver.Solve2(ValidFile);

            result.ShouldBe(4);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var result = _solver.Solve2(PuzzleFile);

            Console.WriteLine($"PassportProcessing Part 2: {result}");
        }
    }
}
