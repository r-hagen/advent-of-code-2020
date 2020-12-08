using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class HandheldHalting
    {
        public int Solve1(string[] commands)
        {
            var instructions = commands
                .Select(cmd => Instruction.Create(cmd))
                .ToArray();

            var history = new List<int>();

            var accumulator = 0;
            var index = 0;

            Action<int> moveIndex = (int amount) =>
            {
                if (index + amount >= instructions.Length)
                {
                    index = (index + amount) - instructions.Length;
                }
                else if (index + amount < 0)
                {
                    index = 0;
                }
                else
                {
                    index = index + amount;
                }
            };

            while (true)
            {
                if (history.Contains(index))
                    break;

                history.Add(index);

                var instruction = instructions[index];
                switch (instruction.Command)
                {
                    case "nop":
                        moveIndex(1);
                        continue;

                    case "acc":
                        accumulator += instruction.Argument;
                        moveIndex(1);
                        break;

                    case "jmp":
                        moveIndex(instruction.Argument);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return accumulator;
        }

        public int Solve2(string[] commands)
        {
            var instructions = commands
                .Select(cmd => Instruction.Create(cmd))
                .ToArray();

            var history = new List<int>();

            var fixIndex = -1;
            var fixes = new List<int>();
            var backup = (Instruction)null;

            var accumulator = 0;
            var index = 0;

            while (true)
            {
                if (index >= instructions.Length)
                    break;

                if (history.Contains(index))
                {
                    history.Clear();

                    if (fixIndex != -1)
                    {
                        instructions[fixIndex] = backup;
                        fixes.Add(fixIndex);
                        fixIndex = -1;
                    }

                    index = 0;
                    accumulator = 0;
                }

                history.Add(index);

                var instruction = instructions[index];

                if (fixIndex == -1 && !fixes.Contains(index))
                {
                    if (instruction.Command == "nop")
                    {
                        backup = instruction;
                        fixIndex = index;
                        instructions[fixIndex] = instruction = instruction.WithCommand("jmp");

                    }
                    else if (instruction.Command == "jmp")
                    {
                        backup = instruction;
                        fixIndex = index;
                        instructions[fixIndex] = instruction = instruction.WithCommand("nop");
                    }
                }

                switch (instruction.Command)
                {
                    case "nop":
                        index++;
                        continue;

                    case "acc":
                        accumulator += instruction.Argument;
                        index++;
                        break;

                    case "jmp":
                        index += instruction.Argument;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return accumulator;
        }

        public class Instruction
        {
            public string Command { get; }
            public int Argument { get; }

            private Instruction(string command, int argument)
            {
                Command = command;
                Argument = argument;
            }

            public static Instruction Create(string input)
            {
                var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var instruction = new Instruction(tokens[0], int.Parse(tokens[1]));
                return instruction;
            }

            public Instruction WithCommand(string command)
            {
                return new Instruction(command, this.Argument);
            }
        }
    }
}
