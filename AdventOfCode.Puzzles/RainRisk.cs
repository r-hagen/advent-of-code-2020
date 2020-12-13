using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class RainRisk
    {
        private readonly char[] Directions = new[] { 'N', 'E', 'S', 'W' };
        private readonly Dictionary<char, int> Position = new Dictionary<char, int>() {
            { 'E', 0 },
            { 'W', 0 },
            { 'N', 0 },
            { 'S', 0 },
        };

        private char FacingTowards = 'E';

        public IEnumerable<(char Action, int Input)> ParseInput(string[] input)
        {
            foreach (var line in input)
            {
                var regex = new Regex($"^(?<action>[NSEWLRF])(?<value>[0-9]+)$");

                var match = regex.Match(line);
                if (!match.Success) continue;

                var action = char.Parse(match.Groups["action"].Value);
                var value = int.Parse(match.Groups["value"].Value);

                yield return (action, value);
            }
        }

        public int Solve1(string[] input)
        {
            var instructions = ParseInput(input);

            foreach (var i in instructions)
            {
                switch (i.Action)
                {
                    case 'F':
                        Position[FacingTowards] += i.Input;
                        break;

                    case 'R':
                        var ir =
                            (Array.IndexOf(Directions, FacingTowards) + (i.Input / 90))
                                % Directions.Length;
                        FacingTowards = Directions[ir];
                        break;

                    case 'L':
                        var il =
                            (Array.IndexOf(Directions, FacingTowards) - (i.Input / 90))
                                % Directions.Length;
                        il = il < 0 ? Directions.Length + il : il;
                        FacingTowards = Directions[il];
                        break;

                    case 'N':
                        Position['N'] += i.Input;
                        break;

                    case 'E':
                        Position['E'] += i.Input;
                        break;

                    case 'S':
                        Position['S'] += i.Input;
                        break;

                    case 'W':
                        Position['W'] += i.Input;
                        break;
                }
            }

            var dist = Math.Abs(Position['N'] - Position['S']) + Math.Abs(Position['E'] - Position['W']);

            return dist;
        }

        public int Solve2(string[] input)
        {
            var instructions = ParseInput(input);

            var waypoint = (p1: (dir: 'E', value: 10), p2: (dir: 'N', value: 1));

            foreach (var i in instructions)
            {
                switch (i.Action)
                {
                    case 'F':
                        Position[waypoint.p1.dir] += waypoint.p1.value * i.Input;
                        Position[waypoint.p2.dir] += waypoint.p2.value * i.Input;

                        break;

                    case 'R':
                        var ir1 =
                            (Array.IndexOf(Directions, waypoint.p1.dir) + (i.Input / 90))
                                % Directions.Length;
                        waypoint.p1.dir = Directions[ir1];

                        var ir2 =
                            (Array.IndexOf(Directions, waypoint.p2.dir) + (i.Input / 90))
                                % Directions.Length;
                        waypoint.p2.dir = Directions[ir2];

                        break;

                    case 'L':
                        var il1 =
                            (Array.IndexOf(Directions, waypoint.p1.dir) - (i.Input / 90))
                                % Directions.Length;
                        il1 = il1 < 0 ? Directions.Length + il1 : il1;
                        waypoint.p1.dir = Directions[il1];

                        var il2 =
                            (Array.IndexOf(Directions, waypoint.p2.dir) - (i.Input / 90))
                                % Directions.Length;
                        il2 = il2 < 0 ? Directions.Length + il2 : il2;
                        waypoint.p2.dir = Directions[il2];

                        break;

                    case 'N':
                        if (waypoint.p1.dir == 'N' || waypoint.p1.dir == 'S')
                            if (waypoint.p1.dir == i.Action)
                                waypoint.p1.value += i.Input;
                            else
                                waypoint.p1.value -= i.Input;

                        if (waypoint.p2.dir == 'N' || waypoint.p2.dir == 'S')
                            if (waypoint.p2.dir == i.Action)
                                waypoint.p2.value += i.Input;
                            else
                                waypoint.p2.value -= i.Input;
                        break;
                    case 'S':
                        if (waypoint.p1.dir == 'N' || waypoint.p1.dir == 'S')
                            if (waypoint.p1.dir == i.Action)
                                waypoint.p1.value += i.Input;
                            else
                                waypoint.p1.value -= i.Input;

                        if (waypoint.p2.dir == 'N' || waypoint.p2.dir == 'S')
                            if (waypoint.p2.dir == i.Action)
                                waypoint.p2.value += i.Input;
                            else
                                waypoint.p2.value -= i.Input;
                        break;
                    case 'E':
                        if (waypoint.p1.dir == 'E' || waypoint.p1.dir == 'W')
                            if (waypoint.p1.dir == i.Action)
                                waypoint.p1.value += i.Input;
                            else
                                waypoint.p1.value -= i.Input;

                        if (waypoint.p2.dir == 'E' || waypoint.p2.dir == 'W')
                            if (waypoint.p2.dir == i.Action)
                                waypoint.p2.value += i.Input;
                            else
                                waypoint.p2.value -= i.Input;
                        break;
                    case 'W':
                        if (waypoint.p1.dir == 'E' || waypoint.p1.dir == 'W')
                            if (waypoint.p1.dir == i.Action)
                                waypoint.p1.value += i.Input;
                            else
                                waypoint.p1.value -= i.Input;

                        if (waypoint.p2.dir == 'E' || waypoint.p2.dir == 'W')
                            if (waypoint.p2.dir == i.Action)
                                waypoint.p2.value += i.Input;
                            else
                                waypoint.p2.value -= i.Input;
                        break;
                }
            }

            printPosition();

            var dist = Math.Abs(Position['N'] - Position['S']) + Math.Abs(Position['E'] - Position['W']);

            return dist;
        }

        public void printPosition()
        {
            foreach (var kvp in Position)
            {
                Console.WriteLine($"{kvp.Key} {kvp.Value}");
            }
        }
    }
}
