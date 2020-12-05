using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class ReportRepair
    {
        public int? Solve1(string inputFile)
        {
            int? solution = null;
            var numbers = ParseInput(inputFile);

            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    if (i == j) continue;

                    if (numbers[i] + numbers[j] == 2020)
                        solution = numbers[i] * numbers[j];
                }
            }

            return solution;
        }

        public int? Solve2(string inputFile)
        {
            int? solution = null;
            var numbers = ParseInput(inputFile);

            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    for (int k = 0; k < numbers.Length; k++)
                    {
                        if (i == j || i == k || j == k) continue;

                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                            solution = numbers[i] * numbers[j] * numbers[k];
                    }
                }
            }

            return solution;
        }

        public int[] ParseInput(string inputFile) {
            return File.ReadAllText(inputFile)
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => int.Parse(line.Trim())).ToArray();
        }
    }
}
