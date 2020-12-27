using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Puzzles
{
    public class Node
    {
        public Node(int label)
        {
            Label = label;
            Prev = this;
            Next = this;
        }

        public int Label { get; }
        public Node Next { get; private set; }
        public Node Prev { get; private set; }

        public void Remove()
        {
            Next.Prev = Prev;
            Prev.Next = Next;
            Prev = this;
            Next = this;
        }

        public void InsertBefore(Node other)
        {
            if (other.Prev != other || other.Next != other)
                throw new NotSupportedException();

            Prev.Next = other;
            other.Prev = Prev;
            other.Next = this;
            Prev = other;
        }

        public void InsertAfter(Node other)
        {
            if (other.Prev != other || other.Next != other)
                throw new NotSupportedException();

            Next.Prev = other;
            other.Prev = this;
            other.Next = Next;
            Next = other;
        }
    }

    public static class CrabCups
    {
        public static string Solve1(string input, int moves)
        {
            var cups = ParseCups(input);

            cups = SimulateMoves(moves, cups);

            var labels = new List<int>();
            var startIndex = (cups.IndexOf(1) + 1) % cups.Count;

            for (var i = 0; i < cups.Count - 1; i++)
                labels.Add(cups[(startIndex + i) % cups.Count]);

            return string.Join("", labels);
        }

        private static List<int> ParseCups(string input)
        {
            return input.Select(x => int.Parse(x.ToString())).ToList();
        }

        public static string Solve2(string input, int moves)
        {
            var cupOrder = ParseCups(input);
            cupOrder.AddRange(Enumerable.Range(10, 1_000_000 - cupOrder.Count));

            var nodeDict = Enumerable.Range(1, cupOrder.Count)
                .Select(n => new Node(n))
                .ToDictionary(n => n.Label);

            var nodeList = (Node) default;

            foreach (var label in cupOrder)
                if (nodeList == default) nodeList = nodeDict[label];
                else nodeList.InsertBefore(nodeDict[label]);

            var current = nodeDict[cupOrder[0]];

            for (var i = 1; i <= moves; i++)
            {
                var destination = current.Label;
                var first = current.Next;
                var second = current.Next.Next;
                var third = current.Next.Next.Next;

                var invalid = new[] {current.Label, first.Label, second.Label, third.Label};
                while (invalid.Contains(destination))
                {
                    destination = (destination - 1) % cupOrder.Count;

                    if (destination == 0)
                        destination = cupOrder.Count;
                }

                first.Remove();
                second.Remove();
                third.Remove();

                nodeDict[destination].InsertAfter(third);
                nodeDict[destination].InsertAfter(second);
                nodeDict[destination].InsertAfter(first);

                current = current.Next;
            }

            var nodeOne = nodeDict[1];
            var result = new BigInteger(nodeOne.Next.Label) * new BigInteger(nodeOne.Next.Next.Label);

            return result.ToString();
        }

        private static void Print(Node node)
        {
            var current = node;

            do
            {
                Console.Write($"{current.Label} ");
                current = current.Next;
            } while (current != node);
        }

        private static List<int> SimulateMoves(int moves, List<int> cups)
        {
            var currentCup = (Index: 0, Label: cups[0]);

            for (var i = 1; i <= moves; i++)
            {
                var pickedCups = Enumerable.Range(1, 3)
                    .Select(x => cups[(currentCup.Index + x) % cups.Count])
                    .ToList();

                var leftoverCups = cups.Except(pickedCups).ToList();

                var targetLabel = currentCup.Label - 1;
                while (targetLabel < cups.Min() || !leftoverCups.Contains(targetLabel))
                    targetLabel = targetLabel < cups.Min() ? cups.Max() : targetLabel - 1;
                var targetIndex = leftoverCups.IndexOf(targetLabel);

                var reorderedCups = new List<int>(leftoverCups);
                reorderedCups.InsertRange(targetIndex + 1, pickedCups);

                cups = reorderedCups;
                currentCup = (
                    (cups.IndexOf(currentCup.Label) + 1) % cups.Count,
                    cups[(cups.IndexOf(currentCup.Label) + 1) % cups.Count]
                );
            }

            return cups;
        }
    }
}
