using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class RambunctiousRecitation
    {
        private Dictionary<int, SpokenNumber> _spoken = new();
        private int _turn = 1;
        private int _number = 0;

        public int Solve(string input, int nthTurn)
        {
            var startingNumbers = parseInput(input);

            foreach (var n in startingNumbers)
                speakNumber(n);

            while (_turn <= nthTurn)
            {
                if (!hadBeenSpoken(_number))
                    speakNumber(0);
                else
                    speakNumber(_spoken[_number].LastTurn - _spoken[_number].BeforeLastTurn);
            }

            return _number;
        }

        private int[] parseInput(string input)
        {
            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToArray();
        }

        private void speakNumber(int number)
        {
            _number = number;

            if (!_spoken.ContainsKey(number))
                _spoken.Add(number, new SpokenNumber(number, _turn));
            else
                _spoken[number].SpeakAgain(_turn);

            _turn++;
        }

        private bool hadBeenSpoken(int number)
        {
            if (!_spoken.ContainsKey(number))
                return false;
            else
                return _spoken[number].HadBeenSpoken();
        }
    }

    public record SpokenNumber
    {
        public int Number { get; private set; }
        public int LastTurn { get; private set; }
        public int BeforeLastTurn { get; private set; }
        public int SpokenCount { get; private set; }

        public SpokenNumber(int number, int turn)
        {
            Number = number;
            BeforeLastTurn = 0;
            LastTurn = turn;
            SpokenCount = 1;
        }

        public void SpeakAgain(int turn)
        {
            BeforeLastTurn = LastTurn;
            LastTurn = turn;
            SpokenCount++;
        }

        public bool HadBeenSpoken()
        {
            return SpokenCount > 1;
        }
    }
}
