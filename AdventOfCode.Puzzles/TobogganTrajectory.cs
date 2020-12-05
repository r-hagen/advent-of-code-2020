using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class TobogganTrajectory
    {
        public int Solve1(string inputFile, int right, int down)
        {
            var trees = 0;
            var map = ParseInput(inputFile);

            var y = down;
            var x = right;

            while (y < map.Length)
            {
                if (map[y][x] == '#')
                    trees++;

                if (x + right >= map[y].Length)
                    x = x + right - map[y].Length;
                else
                    x = x + right;

                y = y + down;
            }

            return trees;
        }

        public string[] ParseInput(string inputFile)
        {
            return File.ReadAllLines(inputFile)
                .Select(line => line.Trim())
                .ToArray();
        }
    }
}
