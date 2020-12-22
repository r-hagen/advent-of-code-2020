using System.IO;
using System.Linq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Puzzles.Tests
{
    public class JurassicJigsawTest
    {
        private const string ExampleFile = "Assets/JurassicJigsawExample.txt";
        private const string PuzzleFile = "Assets/JurassicJigsaw.txt";

        private readonly JurassicJigsaw _solver;
        private readonly ITestOutputHelper _testOutputHelper;

        public JurassicJigsawTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _solver = new JurassicJigsaw();
        }

        [Fact]
        public void Tile_FlipX_FlipsTileMatrixInX()
        {
            var flipped = makeTile().Flip('x');

            flipped.Data.ShouldBe(new[,]
            {
                {'7', '8', '9'},
                {'4', '5', '6'},
                {'1', '2', '3'}
            });
        }

        [Fact]
        public void Tile_FlipY_FlipsTileMatrixInY()
        {
            var flipped = makeTile().Flip('y');

            flipped.Data.ShouldBe(new[,]
            {
                {'3', '2', '1'},
                {'6', '5', '4'},
                {'9', '8', '7'}
            });
        }

        [Fact]
        public void Tile_Rotate1_RotatesTileMatrixClockwiseBy90()
        {
            var rotated = makeTile().RotateClockwise(90);

            rotated.Data.ShouldBe(new[,]
            {
                {'7', '4', '1'},
                {'8', '5', '2'},
                {'9', '6', '3'}
            });
        }

        [Fact]
        public void Tile_Rotate2_RotatesTileMatrixClockwiseBy180()
        {
            var rotated = makeTile().RotateClockwise(180);

            rotated.Data.ShouldBe(new[,]
            {
                {'9', '8', '7'},
                {'6', '5', '4'},
                {'3', '2', '1'}
            });
        }

        [Fact]
        public void Tile_Rotate3_RotatesTileMatrixClockwiseBy270()
        {
            var rotated = makeTile().RotateClockwise(270);

            rotated.Data.ShouldBe(new[,]
            {
                {'3', '6', '9'},
                {'2', '5', '8'},
                {'1', '4', '7'}
            });
        }

        [Fact]
        public void Tile_EdgesIndexer_ReturnsExpectedEdge()
        {
            var tile = makeTile();

            tile.Edges[Edge.Top].ShouldBe("123");
            tile.Edges[Edge.Bottom].ShouldBe("789");
            tile.Edges[Edge.Left].ShouldBe("147");
            tile.Edges[Edge.Right].ShouldBe("369");
        }

        [Fact]
        public void Tile_Configurations_HasNoDuplicates()
        {
            var configurations = makeTile().Configurations.ToArray();

            foreach (var tile in configurations)
            {
                var others = configurations.Where(c => c != tile).ToList();

                foreach (var other in others) other.Data.ShouldNotBe(tile.Data);
            }
        }

        [Fact]
        public void Tile_Configurations_GeneratesEightUnique()
        {
            var configurations = makeTile().Configurations.ToArray();

            configurations.Length.ShouldBe(8);
        }

        [Fact]
        public void Tile_ConnectsToEdge_ShouldNotConnect()
        {
            var t1 = makeTile();
            var t2 = makeTile();

            t1.CanConnect(Edge.Top, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Bottom, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Left, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Right, t2).ShouldBeFalse();
        }

        [Fact]
        public void Tile_ConnectsToEdge_ShouldConnectOnOneEdgeWithFlippedX()
        {
            var t1 = makeTile();
            var t2 = t1.Flip('x');

            t1.CanConnect(Edge.Top, t2).ShouldBeTrue();
            t1.CanConnect(Edge.Bottom, t2).ShouldBeTrue();
            t1.CanConnect(Edge.Left, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Right, t2).ShouldBeFalse();
        }

        [Fact]
        public void Tile_ConnectsToEdge_ShouldConnectOnOneEdgeWithFlippedY()
        {
            var t1 = makeTile();
            var t2 = t1.Flip('y');

            t1.CanConnect(Edge.Top, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Bottom, t2).ShouldBeFalse();
            t1.CanConnect(Edge.Left, t2).ShouldBeTrue();
            t1.CanConnect(Edge.Right, t2).ShouldBeTrue();
        }

        [Fact]
        public void Should_solve_example_1()
        {
            var input = File.ReadAllLines(ExampleFile);

            var result = JurassicJigsaw.Solve1(input);

            result.ShouldBe("20899048083289");
        }

        [Fact]
        public void Should_solve_puzzle_1()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = JurassicJigsaw.Solve1(input);

            _testOutputHelper.WriteLine($"JurassicJigsaw Part 1: {result}");
        }

        [Fact]
        public void Should_solve_example_2()
        {
            var input = File.ReadAllLines(ExampleFile);

            var result = JurassicJigsaw.Solve2(input);

            result.ShouldBe("273");
        }

        [Fact]
        public void Should_solve_puzzle_2()
        {
            var input = File.ReadAllLines(PuzzleFile);

            var result = JurassicJigsaw.Solve2(input);

            _testOutputHelper.WriteLine($"JurassicJigsaw Part 2: {result}");
        }

        private Tile makeTile()
        {
            return new(1, new[,]
            {
                {'1', '2', '3'},
                {'4', '5', '6'},
                {'7', '8', '9'}
            });
        }
    }
}