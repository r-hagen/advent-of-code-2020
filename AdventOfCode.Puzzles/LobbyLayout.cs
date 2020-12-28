using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public static class LobbyLayout
    {
        private static Dictionary<(int x, int y), bool> _floorTiles;

        public static string Solve1(string[] input)
        {
            var tilesToFlip = ParseTilesToFlip(input);
            _floorTiles = new Dictionary<(int x, int y), bool>();

            foreach (var tile in tilesToFlip)
            {
                var x = 0;
                var y = 0;

                foreach (var instruction in tile)
                    switch (instruction)
                    {
                        case "se":
                            x += 1;
                            y += 1;
                            break;
                        case "sw":
                            x -= 1;
                            y += 1;
                            break;
                        case "ne":
                            x += 1;
                            y -= 1;
                            break;
                        case "nw":
                            x -= 1;
                            y -= 1;
                            break;
                        case "e":
                            x += 2;
                            break;
                        case "w":
                            x -= 2;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                var coords = (x, y);

                if (!_floorTiles.ContainsKey(coords))
                    _floorTiles.Add(coords, true);
                else
                    _floorTiles[coords] ^= true;
            }

            var blackTiles = _floorTiles.Count(t => t.Value);
            return blackTiles.ToString();
        }

        public static string Solve2(string[] input, int days)
        {
            Solve1(input);

            for (var i = 1; i <= days; i++)
            {
                var flippedTiles = new Dictionary<(int x, int y), bool>();

                foreach (var tile in _floorTiles.Keys)
                {
                    var tns = Neighbors(tile)
                        .Where(t => _floorTiles.ContainsKey(t))
                        .Count(t => _floorTiles[t]);

                    if (_floorTiles[tile])
                    {
                        if (tns == 1 || tns == 2)
                            flippedTiles[tile] = true;
                    }
                    else
                    {
                        if (tns == 2)
                            flippedTiles[tile] = true;
                    }

                    foreach (var n in Neighbors(tile))
                    {
                        var nns = Neighbors(n)
                            .Where(t => _floorTiles.ContainsKey(t))
                            .Count(t => _floorTiles[t]);

                        if (_floorTiles.ContainsKey(n) && _floorTiles[n])
                        {
                            if (nns == 1 || nns == 2)
                                flippedTiles[n] = true;
                        }
                        else
                        {
                            if (nns == 2)
                                flippedTiles[n] = true;
                        }
                    }
                }

                _floorTiles = flippedTiles;

                var blacks = _floorTiles.Count(t => t.Value);
                Console.WriteLine($"Day {i}: {blacks}");
            }

            var result = _floorTiles.Count(t => t.Value);
            return result.ToString();
        }

        private static IEnumerable<(int x, int y)> Neighbors((int x, int y) tile)
        {
            yield return (tile.x + 1, tile.y + 1);
            yield return (tile.x + 1, tile.y - 1);
            yield return (tile.x - 1, tile.y + 1);
            yield return (tile.x - 1, tile.y - 1);
            yield return (tile.x + 2, tile.y);
            yield return (tile.x - 2, tile.y);
        }

        private static List<string[]> ParseTilesToFlip(string[] input)
        {
            var result = new List<string[]>();
            var regex = new Regex("(e|se|sw|w|nw|ne)");

            foreach (var line in input)
            {
                var matches = regex.Matches(line);

                var instructions = matches.Select(m => m.Value).ToArray();
                result.Add(instructions);

                Debug.Assert(string.Join("", instructions) == line);
            }

            return result;
        }
    }
}
