using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class JurassicJigsaw
    {
        public static string Solve1(IEnumerable<string> input)
        {
            var allTiles = ParseTiles(input).SelectMany(t => t.Configurations).ToArray();

            ConnectAllTiles(allTiles);

            var corners = allTiles.Where(t => t.ConnectedCount == 2).Select(t => t.Id).ToList();
            var result = corners.Aggregate((long) 1, (acc, n) => acc * n);

            return result.ToString();
        }

        public static string Solve2(IEnumerable<string> input)
        {
            var inputTiles = ParseTiles(input).ToArray();
            var allTiles = inputTiles.SelectMany(t => t.Configurations).ToArray();

            ConnectAllTiles(allTiles);

            var topLeft = GetTopLeftCornerTile(allTiles);
            var imageSize = (int) Math.Sqrt(inputTiles.Length);

            var imageGrid = CreateImageFromTopleftCornerTile(imageSize, topLeft);
            var tile = CreateImageTile(imageSize, imageGrid);

            var monsterMask = new[]
            {
                "                  # ".ToCharArray(),
                "#    ##    ##    ###".ToCharArray(),
                " #  #  #  #  #  #   ".ToCharArray()
            };

            var maskIndices = monsterMask.SelectMany((row, dy) =>
                    row.Select((_, dx) => (dy, dx)).Where(x => row[x.dx] == '#'))
                .ToArray();

            var dymax = maskIndices.Select(x => x.dy).Max();
            var dxmax = maskIndices.Select(x => x.dx).Max();

            var monsters = 0;
            var hashCount = 0;

            foreach (var c in tile.Configurations)
            {
                monsters = 0;
                hashCount = 0;

                for (var i = 0; i < c.Size; i++)
                for (var j = 0; j < c.Size; j++)
                {
                    if (c.Data[i, j] == '#')
                        hashCount++;

                    if (i + dymax >= c.Size || j + dxmax >= c.Size)
                        continue;

                    if (maskIndices.All(t => c.Data[i + t.dy, j + t.dx] == '#')) monsters++;
                }

                if (monsters > 0)
                    break;
            }

            var hashesNotPartOfSeaMonster = hashCount - monsters * maskIndices.Length;

            return hashesNotPartOfSeaMonster.ToString();
        }

        private static void ConnectAllTiles(Tile[] allTiles)
        {
            var possibleEdges = new[] {Edge.Top, Edge.Bottom, Edge.Left, Edge.Right};
            var tilesToSolve = new Queue<Tile>();
            tilesToSolve.Enqueue(allTiles.First());

            while (tilesToSolve.Count > 0)
            {
                var currentTile = tilesToSolve.Dequeue();

                foreach (var tile in allTiles)
                {
                    if (tile.Id == currentTile.Id)
                        continue;

                    foreach (var edge in possibleEdges)
                    {
                        if (currentTile.HasConnection(edge))
                            continue;

                        if (tile.HasOppositeConnection(edge))
                            continue;

                        if (!currentTile.CanConnect(edge, tile))
                            continue;

                        currentTile.Connect(edge, tile);
                        tilesToSolve.Enqueue(tile);

                        break;
                    }
                }
            }
        }

        private static Tile CreateImageTile(int imageSize, Tile[,] imageGrid)
        {
            var borderlessGrid = RemoveTileBorders(imageSize, imageGrid);

            var borderlessSize = borderlessGrid[0, 0].Size;
            var combinedSize = borderlessSize * imageSize;

            var combined = new char[combinedSize, combinedSize];

            for (var i = 0; i < imageSize; i++)
            for (var j = 0; j < imageSize; j++)
            for (var r = 0; r < borderlessSize; r++)
            for (var c = 0; c < borderlessSize; c++)
            {
                var rowOffset = i * borderlessSize + r;
                var colOffset = j * borderlessSize + c;

                combined[rowOffset, colOffset] = borderlessGrid[i, j].Data[r, c];
            }

            return new Tile(-1, combined);
        }

        private static Tile[,] RemoveTileBorders(int imageSize, Tile[,] image)
        {
            var borderless = new Tile[imageSize, imageSize];

            for (var i = 0; i < imageSize; i++)
            for (var j = 0; j < imageSize; j++)
                borderless[i, j] = image[i, j].RemoveBorder();

            return borderless;
        }

        private static Tile[,] CreateImageFromTopleftCornerTile(int imageSize, Tile topLeft)
        {
            var image = new Tile[imageSize, imageSize];
            image[0, 0] = topLeft;

            for (var i = 1; i < imageSize; i++)
                image[i, 0] = image[i - 1, 0].Connections[Edge.Bottom];

            for (var i = 0; i < imageSize; i++)
            for (var j = 1; j < imageSize; j++)
                image[i, j] = image[i, j - 1].Connections[Edge.Right];

            return image;
        }

        private static Tile GetTopLeftCornerTile(IEnumerable<Tile> tiles)
        {
            return tiles.Single(t =>
                t.ConnectedCount == 2 && !t.HasConnection(Edge.Top) && !t.HasConnection(Edge.Left));
        }

        private static IEnumerable<Tile> ParseTiles(IEnumerable<string> tileInput)
        {
            var tiles = new List<Tile>();
            var tile = (Tile) default;
            var tileRow = 0;

            foreach (var line in tileInput)
                if (line.StartsWith("Tile"))
                {
                    tile = new Tile(ParseTileId(line));
                    tiles.Add(tile);
                    tileRow = 0;
                }
                else if (!line.Equals(string.Empty))
                {
                    tile!.SetRow(line, tileRow++);
                }

            return tiles;
        }

        private static int ParseTileId(string line)
        {
            var regex = new Regex(@"^Tile (?<id>\d+):$");

            var match = regex.Match(line);
            if (!match.Success) throw new NotSupportedException();

            return int.Parse(match.Groups["id"].Value);
        }
    }

    public enum Edge
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public class Tile
    {
        private static readonly Dictionary<Edge, Edge> OppositeEdges = new()
        {
            {Edge.Top, Edge.Bottom},
            {Edge.Bottom, Edge.Top},
            {Edge.Left, Edge.Right},
            {Edge.Right, Edge.Left}
        };

        private readonly string _flip = "0";
        private readonly string _rotation = "0";


        public Tile(int id)
        {
            Id = id;
        }

        public Tile(int id, char[,] data)
        {
            (Id, Data) = (id, data);
        }

        private Tile(int id, char[,] data, string rotation, string flip)
        {
            (Id, Data, _rotation, _flip) = (id, data, rotation, flip);
        }


        public char[,] Data { get; private set; }
        public Dictionary<Edge, string> Edges => GetEdges();
        public Dictionary<Edge, Tile> Connections { get; } = new();

        public IEnumerable<Tile> Configurations => CreateConfigurations();
        public int ConnectedCount => Connections.Keys.Count;
        public int Id { get; }
        public int Size => Data.GetLength(0);

        public bool HasConnection(Edge edge)
        {
            return Connections.ContainsKey(edge);
        }

        public bool HasOppositeConnection(Edge edge)
        {
            return HasConnection(OppositeEdges[edge]);
        }

        public void Connect(Edge edge, Tile tile)
        {
            Connections.Add(edge, tile);
            tile.Connections.Add(OppositeEdges[edge], this);
        }

        public void SetRow(string data, int row)
        {
            Data ??= new char[data.Length, data.Length];

            for (var i = 0; i < data.Length; i++)
                Data[row, i] = data[i];
        }

        public Tile RotateClockwise(float degrees)
        {
            if (degrees == 0)
                return this;

            var rotations = degrees / 360 * 4;
            var rotated = Clone(Data);

            for (var r = 0; r < rotations; r++)
            {
                var tmp = Clone(rotated);

                for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                    rotated[i, j] = tmp[Size - j - 1, i];
            }

            return new Tile(Id, rotated, degrees.ToString(CultureInfo.InvariantCulture), _flip);
        }

        public Tile Flip(char flipAxis)
        {
            var flipped = new char[Size, Size];

            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                flipped[i, j] = flipAxis switch
                {
                    'x' => Data[Size - i - 1, j],
                    'y' => Data[i, Size - j - 1],
                    _ => throw new ArgumentOutOfRangeException(nameof(flipAxis))
                };

            return new Tile(Id, flipped, _rotation, flipAxis.ToString());
        }

        public bool CanConnect(Edge edge, Tile tile)
        {
            var sourceEdge = Edges[edge];

            var targetEdge = edge switch
            {
                Edge.Top => tile.Edges[Edge.Bottom],
                Edge.Bottom => tile.Edges[Edge.Top],
                Edge.Left => tile.Edges[Edge.Right],
                Edge.Right => tile.Edges[Edge.Left],
                _ => throw new ArgumentOutOfRangeException(nameof(edge))
            };

            return sourceEdge.Equals(targetEdge);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"id: {Id}, rotation: {_rotation}, flip: {_flip}");

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++) sb.Append(Data[i, j]);

                if (i != Size - 1)
                    sb.Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        public Tile RemoveBorder()
        {
            var borderlessSize = Size - 2;
            var borderless = new char[borderlessSize, borderlessSize];

            for (var i = 1; i < Size - 1; i++)
            for (var j = 1; j < Size - 1; j++)
                borderless[i - 1, j - 1] = Data[i, j];

            return new Tile(Id, borderless);
        }

        private Dictionary<Edge, string> GetEdges()
        {
            return new()
            {
                {Edge.Top, new string(GetTopEdge())},
                {Edge.Right, new string(GetRightEdge())},
                {Edge.Bottom, new string(GetBottomEdge())},
                {Edge.Left, new string(GetLeftEdge())}
            };
        }

        private IEnumerable<Tile> CreateConfigurations()
        {
            var configurations = new List<Tile>();

            for (var i = 0; i < 360; i += 90)
                configurations.Add(RotateClockwise(i));

            for (var i = 0; i < 180; i += 90)
                configurations.Add(Flip('x').RotateClockwise(i));

            for (var i = 0; i < 180; i += 90)
                configurations.Add(Flip('y').RotateClockwise(i));

            return configurations;
        }

        private char[] GetTopEdge()
        {
            var top = new char[Size];
            for (var i = 0; i < Size; i++)
                top[i] = Data[0, i];
            return top;
        }

        private char[] GetBottomEdge()
        {
            var bottom = new char[Size];
            for (var i = 0; i < Size; i++)
                bottom[i] = Data[Size - 1, i];
            return bottom;
        }

        private char[] GetLeftEdge()
        {
            var left = new char[Size];
            for (var i = 0; i < Size; i++)
                left[i] = Data[i, 0];
            return left;
        }

        private char[] GetRightEdge()
        {
            var right = new char[Size];
            for (var i = 0; i < Size; i++)
                right[i] = Data[i, Size - 1];
            return right;
        }

        private static char[,] Clone(char[,] arr2d)
        {
            return arr2d.Clone() as char[,];
        }
    }
}