using System;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class SeatingSystem
    {
        private bool _seatMoved = false;

        public int Solve1(char[][] input)
        {
            var moved = input.Select(a => a.ToArray()).ToArray();

            do
            {
                _seatMoved = false;
                moved = MoveSeats1(moved);
            }
            while (_seatMoved);

            var seatsOccupied = moved.SelectMany(seat => seat).Where(seat => isOccupied(seat)).Count();
            return seatsOccupied;
        }

        public char[][] MoveSeats1(char[][] seats)
        {
            var movedSeats = seats.Select(a => a.ToArray()).ToArray();

            for (int y = 0; y < seats.Length; y++)
                for (int x = 0; x < seats[y].Length; x++)
                {
                    var seat = seats[y][x];

                    if (isFloor(seat))
                        continue;

                    if (isEmpty(seat))
                    {
                        if (occupiedAdjacents1(seats, y, x) == 0)
                        {
                            movedSeats[y][x] = '#';
                            _seatMoved = true;
                        }
                    }
                    else if (isOccupied(seat))
                    {
                        if (occupiedAdjacents1(seats, y, x) >= 4)
                        {
                            movedSeats[y][x] = 'L';
                            _seatMoved = true;
                        }
                    }
                }

            return movedSeats;
        }

        private int occupiedAdjacents1(char[][] seats, int y, int x)
        {
            var ly = (y - 1) < 0 ? 0 : y - 1;
            var uy = (y + 1) >= seats.Length ? seats.Length - 1 : y + 1;
            var lx = (x - 1) < 0 ? 0 : x - 1;
            var ux = (x + 1) >= seats[0].Length ? seats[0].Length - 1 : x + 1;

            var count = 0;

            for (int r = ly; r <= uy; r++)
            {
                for (int c = lx; c <= ux; c++)
                {
                    if (r == y && c == x)
                        continue;

                    if (isOccupied(seats[r][c]))
                        count++;
                }
            }

            return count;
        }

        private bool isFloor(char seat)
        {
            return seat == '.';
        }

        private bool isEmpty(char seat)
        {
            return seat == 'L';
        }

        private bool isOccupied(char seat)
        {
            return seat == '#';
        }

        private bool isSeat(char seat)
        {
            return isEmpty(seat) || isOccupied(seat);
        }

        public int Solve2(char[][] input)
        {
            var moved = input.Select(a => a.ToArray()).ToArray();

            do
            {
                _seatMoved = false;
                moved = MoveSeats2(moved);
            }
            while (_seatMoved);

            var seatsOccupied = moved.SelectMany(seat => seat).Where(seat => isOccupied(seat)).Count();
            return seatsOccupied;
        }

        public char[][] MoveSeats2(char[][] seats)
        {
            var movedSeats = seats.Select(a => a.ToArray()).ToArray();

            for (int y = 0; y < seats.Length; y++)
                for (int x = 0; x < seats[y].Length; x++)
                {
                    var seat = seats[y][x];

                    if (isFloor(seat))
                        continue;

                    if (isEmpty(seat))
                    {
                        if (visibleOccupied(seats, y, x) == 0)
                        {
                            movedSeats[y][x] = '#';
                            _seatMoved = true;
                        }
                    }
                    else if (isOccupied(seat))
                    {
                        if (visibleOccupied(seats, y, x) >= 5)
                        {
                            movedSeats[y][x] = 'L';
                            _seatMoved = true;
                        }
                    }
                }

            return movedSeats;
        }

        private int visibleOccupied(char[][] seats, int y, int x)
        {
            var count = 0;

            count += countHorizontalAdjacents(seats, y, x);
            count += countVerticalAdjacents(seats, y, x);
            count += countDiagonalAdjacents(seats, y, x);

            return count;
        }

        private int countHorizontalAdjacents(char[][] seats, int y, int x)
        {
            var max = seats[0].Length;
            var row = seats[y];

            var count = 0;

            var lx = x - 1;
            var rx = x + 1;
            Action moveLeft = () => lx = lx - 1;
            Action moveRight = () => rx = rx + 1;

            Func<int, bool> isValid = (int dx) => dx >= 0 && dx < max;

            while (isValid(lx))
            {
                if (isSeat(row[lx]))
                {
                    if (isOccupied(row[lx]))
                    {
                        count++;
                    }
                    break;
                }
                moveLeft();
            }

            while (isValid(rx))
            {
                if (isSeat(row[rx]))
                {
                    if (isOccupied(row[rx]))
                    {
                        count++;
                    }
                    break;
                }
                moveRight();
            }

            if (count > 2) throw new InvalidOperationException();
            return count;
        }

        private int countVerticalAdjacents(char[][] seats, int y, int x)
        {
            var max = seats.Length;

            var count = 0;

            var up = y - 1;
            var down = y + 1;
            Action moveUp = () => up = up - 1;
            Action moveDown = () => down = down + 1;

            Func<int, bool> isValid = (int dy) => dy >= 0 && dy < max;

            while (isValid(up))
            {
                if (isSeat(seats[up][x]))
                {
                    if (isOccupied(seats[up][x]))
                    {
                        count++;
                    }
                    break;
                }
                moveUp();
            }

            while (isValid(down))
            {
                if (isSeat(seats[down][x]))
                {
                    if (isOccupied(seats[down][x]))
                    {
                        count++;
                    }
                    break;
                }
                moveDown();
            }

            if (count > 2) throw new InvalidOperationException();
            return count;
        }

        private int countDiagonalAdjacents(char[][] seats, int y, int x)
        {
            var rowSize = seats.Length;
            var colSize = seats[0].Length;

            var count = 0;

            Func<(int, int), bool> isValid = ((int dy, int dx) coords) =>
                coords.dy >= 0 && coords.dy < rowSize &&
                coords.dx >= 0 && coords.dx < colSize;

            Func<(int, int), char> seatAt = ((int dy, int dx) coords) => seats[coords.dy][coords.dx];

            var upRight = (y - 1, x + 1);
            var downLeft = (y + 1, x - 1);
            var upLeft = (y - 1, x - 1);
            var downRight = (y + 1, x + 1);
            Action moveUpRight = () => upRight = (upRight.Item1 - 1, upRight.Item2 + 1);
            Action moveDownLeft = () => downLeft = (downLeft.Item1 + 1, downLeft.Item2 - 1);
            Action moveUpLeft = () => upLeft = (upLeft.Item1 - 1, upLeft.Item2 - 1);
            Action moveDownRight = () => downRight = (downRight.Item1 + 1, downRight.Item2 + 1);

            while (isValid(upRight))
            {
                if (isSeat(seatAt(upRight)))
                {
                    if (isOccupied(seatAt(upRight)))
                    {
                        count++;
                    }
                    break;
                }
                moveUpRight();
            }

            while (isValid(downLeft))
            {
                if (isSeat(seatAt(downLeft)))
                {
                    if (isOccupied(seatAt(downLeft)))
                    {
                        count++;
                    }
                    break;
                }
                moveDownLeft();
            }

            while (isValid(upLeft))
            {
                if (isSeat(seatAt(upLeft)))
                {
                    if (isOccupied(seatAt(upLeft)))
                    {
                        count++;
                    }
                    break;
                }
                moveUpLeft();
            }

            while (isValid(downRight))
            {
                if (isSeat(seatAt(downRight)))
                {
                    if (isOccupied(seatAt(downRight)))
                    {
                        count++;
                    }
                    break;
                }
                moveDownRight();
            }

            if (count > 4) throw new InvalidOperationException();
            return count;
        }
    }
}
