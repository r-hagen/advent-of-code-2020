using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class CrabCombat
    {
        public static string Solve1(string[] input)
        {
            var playerDecks = ParseDecks(input);

            while (playerDecks.All(deck => deck.Value.Count > 0))
            {
                var p1 = playerDecks[1].Dequeue();
                var p2 = playerDecks[2].Dequeue();

                if (p1 > p2)
                {
                    playerDecks[1].Enqueue(p1);
                    playerDecks[1].Enqueue(p2);
                }
                else
                {
                    playerDecks[2].Enqueue(p2);
                    playerDecks[2].Enqueue(p1);
                }
            }

            var score = CalculateScore(playerDecks);

            return score.ToString();
        }

        public static string Solve2(string[] input)
        {
            var playerDecks = ParseDecks(input);

            RecurseGame(playerDecks);

            var score = CalculateScore(playerDecks);

            return score.ToString();
        }

        private static int GetWinner(Dictionary<int, Queue<int>> playerDecks)
        {
            return playerDecks.Single(deck => deck.Value.Count > 0).Key;
        }

        private static int CalculateScore(Dictionary<int, Queue<int>> playerDecks)
        {
            var winner = GetWinner(playerDecks);
            var score = playerDecks[winner]
                .Reverse()
                .Select((card, index) => (Value: card, Mult: index + 1))
                .Aggregate(0, (total, card) => total += card.Value * card.Mult);
            return score;
        }

        private static Dictionary<int, Queue<int>> ParseDecks(string[] input)
        {
            var playerDecks = new Dictionary<int, Queue<int>>();

            foreach (var line in input)
            {
                if (line == string.Empty) continue;

                if (line.StartsWith("Player"))
                    playerDecks.Add(int.Parse(line.Substring(line.Length - 2, 1)), new Queue<int>());
                else
                    playerDecks.Last().Value.Enqueue(int.Parse(line));
            }

            return playerDecks;
        }

        private static int RecurseGame(Dictionary<int, Queue<int>> playerDecks)
        {
            var p1Prev = new List<int[]>();
            var p2Prev = new List<int[]>();

            while (playerDecks.All(deck => deck.Value.Count > 0))
            {
                if (p1Prev.Any(x => x.SequenceEqual(playerDecks[1])) &&
                    p2Prev.Any(x => x.SequenceEqual(playerDecks[2])))
                    return 1;

                p1Prev.Add(playerDecks[1].ToArray());
                p2Prev.Add(playerDecks[2].ToArray());

                var p1 = playerDecks[1].Dequeue();
                var p2 = playerDecks[2].Dequeue();

                if (playerDecks[1].Count >= p1 && playerDecks[2].Count >= p2)
                {
                    var copyDecks = new Dictionary<int, Queue<int>>
                    {
                        [1] = new(playerDecks[1].Take(p1).ToArray()),
                        [2] = new(playerDecks[2].Take(p2).ToArray())
                    };

                    var winner = RecurseGame(copyDecks);

                    if (winner == 1)
                    {
                        playerDecks[1].Enqueue(p1);
                        playerDecks[1].Enqueue(p2);
                    }
                    else
                    {
                        playerDecks[2].Enqueue(p2);
                        playerDecks[2].Enqueue(p1);
                    }
                }
                else
                {
                    if (p1 > p2)
                    {
                        playerDecks[1].Enqueue(p1);
                        playerDecks[1].Enqueue(p2);
                    }
                    else
                    {
                        playerDecks[2].Enqueue(p2);
                        playerDecks[2].Enqueue(p1);
                    }
                }
            }

            return GetWinner(playerDecks);
        }
    }
}