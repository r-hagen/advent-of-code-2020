using System;
using System.IO;
using System.Linq;
using Xunit;
using Shouldly;

namespace AdventOfCode.Puzzles.Tests
{
    public class SeatingSystemTest
    {
        public const string PuzzleFile = "Assets/SeatingSystem.txt";

        private readonly SeatingSystem _solver;

        public SeatingSystemTest()
        {
            _solver = new SeatingSystem();
        }

        [Fact]
        public void Move_seats_should_equal_example_1()
        {
            var initial = new[] {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            }.Select(line => line.ToCharArray()).ToArray();

            var expected1 = new[] {
                "#.##.##.##",
                "#######.##",
                "#.#.#..#..",
                "####.##.##",
                "#.##.##.##",
                "#.#####.##",
                "..#.#.....",
                "##########",
                "#.######.#",
                "#.#####.##"
            }.Select(line => line.ToCharArray()).ToArray();

            var expected2 = new[] {
                "#.LL.L#.##",
                "#LLLLLL.L#",
                "L.L.L..L..",
                "#LLL.LL.L#",
                "#.LL.LL.LL",
                "#.LLLL#.##",
                "..L.L.....",
                "#LLLLLLLL#",
                "#.LLLLLL.L",
                "#.#LLLL.##"
            }.Select(line => line.ToCharArray()).ToArray();

            var moved = _solver.MoveSeats1(initial);
            /* printSeats(moved); */
            moved.ShouldBe(expected1);

            moved = _solver.MoveSeats1(moved);
            /* printSeats(moved); */
            moved.ShouldBe(expected2);
        }

        private void printSeats(char[][] seats)
        {
            for (int y = 0; y < seats.Length; y++)
            {
                for (int x = 0; x < seats[y].Length; x++)
                {
                    Console.Write(seats[y][x]);
                }
                Console.Write(Environment.NewLine);
            }
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile).Select(line => line.ToCharArray()).ToArray();

            var result = _solver.Solve1(input);

            Console.WriteLine($"SeatingSystem Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_step_1()
        {
            var initial = new[] {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            }.Select(line => line.ToCharArray()).ToArray();

            var expected = new[] {
                "#.##.##.##",
                "#######.##",
                "#.#.#..#..",
                "####.##.##",
                "#.##.##.##",
                "#.#####.##",
                "..#.#.....",
                "##########",
                "#.######.#",
                "#.#####.##"
            }.Select(line => line.ToCharArray()).ToArray();

            var moved = _solver.MoveSeats2(initial);

            moved.ShouldBe(expected);
        }

        [Fact]
        public void Should_solve_example_step_2()
        {
            var initial = new[] {
                "#.##.##.##",
                "#######.##",
                "#.#.#..#..",
                "####.##.##",
                "#.##.##.##",
                "#.#####.##",
                "..#.#.....",
                "##########",
                "#.######.#",
                "#.#####.##"
            }.Select(line => line.ToCharArray()).ToArray();

            var expected = new[] {
                "#.LL.LL.L#",
                "#LLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLL#",
                "#.LLLLLL.L",
                "#.LLLLL.L#"
            }.Select(line => line.ToCharArray()).ToArray();

            var moved = _solver.MoveSeats2(initial);

            moved.ShouldBe(expected);
        }

        [Fact]
        public void Should_solve_example_step_3()
        {
            var initial = new[] {
                "#.LL.LL.L#",
                "#LLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLL#",
                "#.LLLLLL.L",
                "#.LLLLL.L#"
            }.Select(line => line.ToCharArray()).ToArray();

            var expected = new[] {
                "#.L#.##.L#",
                "#L#####.LL",
                "L.#.#..#..",
                "##L#.##.##",
                "#.##.#L.##",
                "#.#####.#L",
                "..#.#.....",
                "LLL####LL#",
                "#.L#####.L",
                "#.L####.L#"
            }.Select(line => line.ToCharArray()).ToArray();

            var moved = _solver.MoveSeats2(initial);

            moved.ShouldBe(expected);
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile).Select(line => line.ToCharArray()).ToArray();

            var result = _solver.Solve2(input);

            Console.WriteLine($"SeatingSystem Part 2: {result}");
        }
    }
}
