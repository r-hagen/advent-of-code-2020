using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class PasswordPhilosophy
    {
        public int Solve1(string inputFile)
        {
            var valid = 0;
            var policies = ParseInput(inputFile);

            foreach (var p in policies)
            {
                var count = p.Password.Count(c => c == p.Letter);

                if (count >= p.Lowest && count <= p.Highest)
                    valid++;
            }

            return valid;
        }

        public int Solve2(string inputFile)
        {
            var valid = 0;
            var policies = ParseInput(inputFile);

            foreach (var p in policies)
            {
                if (p.Password[p.Lowest - 1] == p.Letter ^ p.Password[p.Highest - 1] == p.Letter)
                    valid++;
            }

            return valid;
        }

        public PasswordPolicy[] ParseInput(string inputFile)
        {
            return File.ReadAllLines(inputFile)
                .Select(line => mapToPolicy(line))
                .ToArray();
        }

        private PasswordPolicy mapToPolicy(string line)
        {
            var segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var policySegments = segments[0].Split('-', StringSplitOptions.RemoveEmptyEntries);

            var result = new PasswordPolicy();
            result.Lowest = int.Parse(policySegments[0]);
            result.Highest = int.Parse(policySegments[1]);
            result.Letter = char.Parse(segments[1].Replace(":", ""));
            result.Password = segments[2];

            return result;
        }
    }

    public record PasswordPolicy
    {
        public int Lowest;
        public int Highest;
        public char Letter;
        public string Password;
    }
}
