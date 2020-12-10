using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class AdapterArray
    {
        public int Solve1(int[] adapterJoltages)
        {
            var sortedJoltages = adapterJoltages.OrderBy(x => x).ToArray();

            var inputJoltage = 0;
            var joltageRange = 3;

            var oneJolts = 0;
            var threeJolts = 0;

            var index = 0;

            while (index < sortedJoltages.Length)
            {
                var lowestJoltage = sortedJoltages
                    .Skip(index)
                    .FirstOrDefault(joltage => joltage - inputJoltage <= joltageRange);

                if (lowestJoltage == default)
                    break;

                switch (lowestJoltage - inputJoltage)
                {
                    case 1:
                        oneJolts++;
                        break;
                    case 3:
                        threeJolts++;
                        break;
                }

                inputJoltage = lowestJoltage;
                index = Array.IndexOf(sortedJoltages, lowestJoltage) + 1;
            }

            threeJolts = threeJolts == 0 ? 0 : threeJolts + 1;

            return oneJolts * threeJolts;
        }

        public ulong Solve2(int[] adapterJoltages)
        {
            var sortedJoltages = adapterJoltages.OrderBy(x => x).ToArray();

            var inputJoltage = 0;
            var joltageRange = 3;

            var index = 0;
            ulong count = 0;

            while (index < sortedJoltages.Length)
            {
                var lowestJoltage = sortedJoltages
                    .Skip(index)
                    .FirstOrDefault(joltage => joltage - inputJoltage <= joltageRange);

                if (lowestJoltage == default)
                    break;

                inputJoltage = lowestJoltage;
                index = Array.IndexOf(sortedJoltages, lowestJoltage) + 1;
            }

            inputJoltage += 3;

            var visited = new Dictionary<int, ulong>();

            count = countArrangements(sortedJoltages, 0, 0, inputJoltage, visited);

            return count;
        }

        public ulong countArrangements(int[] adapterJoltages, int index, int inputJoltage, int targetJoltage, Dictionary<int, ulong> visited)
        {
            if (visited.ContainsKey(index))
                return visited[index];

            ulong count = 0;

            var supportedJoltages = adapterJoltages
                .Skip(index)
                .Where(joltage => joltage - inputJoltage <= 3)
                .OrderBy(joltage => joltage);

            if (!supportedJoltages.Any())
                return count;

            foreach (var joltage in supportedJoltages)
            {
                if (joltage + 3 == targetJoltage)
                {
                    count++;
                }
                else
                {
                    var newindex = Array.IndexOf(adapterJoltages, joltage) + 1;
                    count += countArrangements(adapterJoltages, newindex, joltage, targetJoltage, visited);
                }
            }

            visited.Add(index, count);

            return count;
        }
    }
}
