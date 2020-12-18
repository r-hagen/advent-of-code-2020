using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Puzzles
{
    public class ConwayCubes
    {
        public int Solve(string[] input, int cycles)
        {
            var dimension = parseInput(input);

            for (var cycle = 0; cycle < cycles; cycle++)
                dimension = dimension.Next();

            return dimension.ActiveCount;
        }

        public PocketDimension parseInput(string[] input)
        {
            var cubes = new List<Cube>();

            for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                    if (isActive(input[y][x]))
                        cubes.Add(makeCube(x, y));

            return makeDimension(cubes.ToArray());
        }

        private Cube makeCube(int x, int y, int z = 0, int w = 0)
        {
            return new Cube(x, y, z, w);
        }

        private PocketDimension makeDimension(params Cube[] cubes)
        {
            return new PocketDimension(cubes);
        }

        public readonly struct Cube
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Z;
            public readonly int W;

            public Cube(int x, int y, int z, int w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }
        }

        public class PocketDimension
        {
            private readonly HashSet<Cube> _activeCubes = new();

            public int ActiveCount => _activeCubes.Count;
            public int LayerCount => _activeCubes.Select(cube => cube.Z).Distinct().Count();

            public int LowestLayer => _activeCubes.Min(cube => cube.Z);
            public int HighestLayer => _activeCubes.Max(cube => cube.Z);
            public int LowestHyper => _activeCubes.Min(cube => cube.W);
            public int HighestHyper => _activeCubes.Max(cube => cube.W);

            public PocketDimension(params Cube[] cubes)
            {
                foreach (var cell in cubes)
                    _activeCubes.Add(cell);
            }

            public bool IsActive(Cube cube)
            {
                return IsActive(cube.X, cube.Y, cube.Z, cube.W);
            }

            public bool IsActive(int x, int y, int z, int w)
            {
                return _activeCubes.Contains(cubeAt(x, y, z, w));
            }

            public int Neighbors(Cube cube)
            {
                var count = 0;

                for (var dx = -1; dx <= 1; dx++)
                    for (var dy = -1; dy <= 1; dy++)
                        for (var dz = -1; dz <= 1; dz++)
                            for (var dw = -1; dw <= 1; dw++)
                            {
                                if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                                    continue;

                                if (IsActive(cube.X + dx, cube.Y + dy, cube.Z + dz, cube.W + dw))
                                    count++;
                            }

                return count;
            }

            public PocketDimension Next()
            {
                var aliveInNext = new HashSet<Cube>();

                foreach (var cube in _activeCubes)
                {
                    for (var dx = -1; dx <= 1; dx++)
                        for (var dy = -1; dy <= 1; dy++)
                            for (var dz = -1; dz <= 1; dz++)
                                for (var dw = -1; dw <= 1; dw++)
                                {
                                    var testingCube = cubeAt(cube.X + dx, cube.Y + dy, cube.Z + dz, cube.W + dw);
                                    if (IsActive(testingCube) && Neighbors(testingCube) == 2 || Neighbors(testingCube) == 3)
                                        aliveInNext.Add(testingCube);
                                }
                }

                return new PocketDimension(aliveInNext.ToArray());
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                for (var z = LowestLayer; z <= HighestLayer; z++)
                {
                    for (var w = LowestHyper; w <= HighestHyper; w++)
                    {
                        sb.AppendLine($"z={z} w={w}");

                        var layer = getLayer(z, w);

                        // TODO respect min,max of all cubes not just current layer
                        var (ymin, ymax) = (layer.Min(c => c.Y), layer.Max(c => c.Y));
                        var (xmin, xmax) = (layer.Min(c => c.X), layer.Max(c => c.X));

                        for (var y = ymin; y <= ymax; y++)
                            for (var x = xmin; x <= xmax; x++)
                                sb.Append($"{(IsActive(x, y, z, w) ? '#' : '.')}{(x == xmax ? Environment.NewLine : "")}");

                        sb.AppendLine();

                    }
                }

                return sb.ToString();
            }

            private Cube cubeAt(int x, int y, int z, int w)
            {
                return new Cube(x, y, z, w);
            }

            private IEnumerable<Cube> getLayer(int z, int w)
            {
                return _activeCubes.Where(cube => cube.Z == z && cube.W == w);
            }
        }

        private bool isActive(char c)
        {
            return c == '#';
        }

        private bool isInactive(char c)
        {
            return c == '.';
        }
    }
}
