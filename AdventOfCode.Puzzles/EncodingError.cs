using System;

namespace AdventOfCode.Puzzles
{
    public class EncodingError
    {
        public ulong Solve1(ulong[] input, int preamble)
        {
            for (int i = preamble; i < input.Length; i++)
            {
                var valid = false;

                for (int j = i - preamble; j < i; j++)
                {
                    if (valid) break;

                    for (int k = j + 1; k < i; k++)
                    {
                        if (valid) break;

                        if (input[j] + input[k] == input[i])
                            valid = true;
                    }
                }

                if (!valid)
                    return input[i];
            }

            return 0;
        }

        public ulong Solve2(ulong[] input, int preamble)
        {
            var invalidNumber = Solve1(input, preamble);
            var invalidIndex = Array.IndexOf(input, invalidNumber);

            for (int i = 0; i < invalidIndex; i++)
            {
                ulong sum = 0;
                ulong min = ulong.MaxValue;
                ulong max = ulong.MinValue;

                for (int j = i; j < invalidIndex; j++)
                {
                    ulong summand = input[j];
                    sum += summand;

                    if (summand < min)
                        min = summand;

                    if (summand > max)
                        max = summand;

                    if (sum >= invalidNumber)
                        break;
                }

                if (sum == invalidNumber)
                    return min + max;
            }

            return 0;
        }
    }
}
