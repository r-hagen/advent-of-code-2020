using System;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class BinaryBoarding
    {
        public (int row, int col, int id) Decode(string pass)
        {
            var rowBits = pass.Substring(0, 7);
            var seatBits = pass.Substring(7, 3);

            var row = partition(rowBits, 0, 127);
            var col = partition(seatBits, 0, 7);
            var id = row * 8 + col;

            return (row, col, id);
        }

        private int partition(string bits, int lv, int uv)
        {
            var bit = bits.First();
            var next = bits.Substring(1, bits.Length - 1);

            if (next.Length == 0)
            {
                if (bit == 'L' || bit == 'F')
                    return lv;
                else
                    return uv;
            }

            if (bit == 'L' || bit == 'F')
                return partition(next, lv, uv - (int)Math.Pow(2, next.Length));

            if (bit == 'R' || bit == 'B')
                return partition(next, lv + (int)Math.Pow(2, next.Length), uv);

            return 0;
        }
    }
}
