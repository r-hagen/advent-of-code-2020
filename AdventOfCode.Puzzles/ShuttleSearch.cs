using System;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Puzzles
{
    public class ShuttleSearch
    {
        public int Solve1(string[] input)
        {
            var earliest = int.Parse(input[0]);
            var ids = input[1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(id => id != "x")
                .Select(id => int.Parse(id));

            var departures = ids.Select(bus =>
            {
                for (int time = earliest; time <= earliest + bus; time++)
                    if (time % bus == 0)
                        return (Time: time, Id: bus);

                return default;
            });

            var winner = departures.Min();

            return winner.Id * (winner.Time - earliest);
        }

        public BigInteger Solve2(string input)
        {
            var ids = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.Parse(id == "x" ? "0" : id))
                .ToArray();

            // Chinese remainder theorem
            //
            // x = a (mod n)
            // the number x has a remainder of a when divided by n
            //
            // the bus departing earliest in the sequence has a remainder of 0
            // what is the remainder of the next buses in the sequence with index offsets of 1-7?
            //
            // in the example given:
            // 1068781 => bus 7  => remainder 0
            // +1      => bus 13 => remainder 12 (13 - 1) where 1 is the index offset
            // +4      => bus 59 => remainder 55 (55 - 4) where 4 is the index offset
            // there seems to be a pattern
            //
            // x = a1 (mod n1)
            // ...
            // x = ak (mod nk)
            //
            // NOTE: only works because each pair of moduli are relatively prime (no common factors between them)
            //   => check for this (eg. for 7 and 13 their highest common factor is 1)
            //
            // so when searching for x = 1068781 we know that:
            // x = ak (mod nk)
            // x0 = 0 (mod 7)
            // x1 = 12 (mod 13)
            //   => where x1 is the number in the sequence with index offset 1
            //   => ak = (nk - k)
            // ...
            //
            // N = n1 n2 ... nk
            //
            // Ni = N / ni
            //   => N1 = n2 .. nk (without n1)
            //
            // x = SUM i_to_k (bi Ni xi) (mod N)
            //
            // xi => modular inverse of Ni

            var filtered = ids
                .Select((id, idx) => new { Id = id, Index = idx })
                .Where(x => x.Id != 0);

            var ni = filtered.Select(x => new BigInteger(x.Id)).ToArray();
            var N = ni.Aggregate(new BigInteger(1), (acc, val) => acc * val);
            var Ni = ni.Select(n => N / n).ToArray();
            var ak = filtered.Select(x => new BigInteger(x.Id - x.Index)).ToArray();
            var xi = Ni.Select((a, i) => ModularInverse(a, ni[i])).ToArray();

            var x = new BigInteger(0);
            for (int i = 0; i < ni.Length; i++)
                x += ak[i] * Ni[i] * xi[i];

            // find smallest remainder
            while (x > N)
                x = x % N;

            return x;
        }

        public BigInteger ModularInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}
