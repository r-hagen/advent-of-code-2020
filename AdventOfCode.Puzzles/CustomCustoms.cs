using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class CustomCustoms
    {
        public int Solve1(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            var groups = new List<List<char>>();

            Func<List<char>> prepareGroup = () =>
            {
                var group = new List<char>();
                groups.Add(group);
                return group;
            };

            var group = prepareGroup();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i] == string.Empty)
                {
                    group = prepareGroup();
                    continue;
                }

                group.AddRange(lines[i]);
            }

            return groups.Sum(g => g.Distinct().Count());
        }


        public int Solve2(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            var groups = new List<AnswerGroup>();

            Func<AnswerGroup> prepareAnswerGroup = () =>
            {
                var newGroup = new AnswerGroup();
                groups.Add(newGroup);
                return newGroup;
            };

            var answerGroup = prepareAnswerGroup();
            for (var i = 0; i < lines.Length; i++)
            {
                var answers = lines[i];

                if (answers == string.Empty)
                {
                    answerGroup = prepareAnswerGroup();
                    continue;
                }

                answerGroup.AddMemberAnswers(answers);
            }

            return groups.Sum(group => group.CountUnanimousAnswers());
        }

        public record AnswerGroup
        {
            private int memberCount = 0;
            private string allAnswers = string.Empty;

            public void AddMemberAnswers(string memberAnswers)
            {
                memberCount++;
                allAnswers += memberAnswers;
            }

            public int CountUnanimousAnswers()
            {
                return allAnswers.Distinct()
                    .Count(question => allAnswers.Where(answer => answer == question).Count() == memberCount);
            }
        }
    }
}
