using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    // NOTE i hate this one so much
    public class OperationOrder
    {
        public ulong Solve1(params string[] input)
        {
            var sum = 0ul;

            Console.WriteLine("-----");

            foreach (var expression in input)
                sum += solveExpression(expression);

            return sum;
        }

        private ulong solveExpression(string expression)
        {
            Console.WriteLine($"exp: {expression}");

            if (expression.StartsWith("("))
            {
                var left = grabParantheses(expression);
                var op = expression[left.Length + 1];
                var remaining = expression.Substring(left.Length + 3);

                left = removeParantheses(left);
                left = solveExpression(left).ToString();

                if (char.IsDigit(remaining[0]))
                    return handleSecondTermIsDigit(remaining, left, op);

                if (remaining.StartsWith("("))
                    return handleSecondTermIsParantheses(remaining, left, op);

                throw new NotImplementedException();
            }

            if (char.IsDigit(expression[0]))
            {
                var spaceIndex = expression.IndexOf(" ", StringComparison.Ordinal);

                var left = expression.Substring(0, spaceIndex);
                var op = expression[spaceIndex + 1];
                var remaining = expression.Substring(spaceIndex + 3); // +3 for space + op + space

                if (char.IsDigit(remaining[0]))
                    return handleSecondTermIsDigit(remaining, left, op);

                if (remaining.StartsWith("("))
                    return handleSecondTermIsParantheses(remaining, left, op);
            }

            throw new NotImplementedException();
        }

        private ulong handleSecondTermIsDigit(string remaining, string left, char op)
        {
            var spaceIndex = remaining.IndexOf(" ", StringComparison.Ordinal);
            if (spaceIndex == -1) return calcExpression(left, remaining, op);

            var right = remaining.Substring(0, spaceIndex);
            remaining = remaining.Substring(spaceIndex);
            left = calcExpression(left, right, op).ToString();

            return solveExpression(left + remaining);
        }

        private ulong handleSecondTermIsParantheses(string remaining, string left, char op)
        {
            var right = grabParantheses(remaining);
            remaining = remaining.Substring(right.Length);

            right = removeParantheses(right);
            right = solveExpression(right).ToString();

            return remaining == string.Empty
                ? calcExpression(left, right, op)
                : solveExpression(calcExpression(left, right, op) + remaining);
        }

        private string removeParantheses(string result)
        {
            result = result.Substring(1);
            result = result.Substring(0, result.Length - 1);
            return result;
        }

        private string grabParantheses(string expression)
        {
            var i = 0;
            var p = 0;

            do
            {
                if (expression[i] == '(')
                    p++;
                else if (expression[i] == ')')
                    p--;

                i++;
            } while (p > 0);

            return expression.Substring(0, i);
        }

        private ulong calcExpression(string left, string right, char op)
        {
            var l = ulong.Parse(left);
            var r = ulong.Parse(right);

            return op switch
            {
                '+' => l + r,
                '*' => l * r,
                _ => throw new NotSupportedException()
            };
        }


        public ulong Solve2(params string[] input)
        {
            var sum = 0ul;

            Console.WriteLine("-----");

            foreach (var expression in input)
                sum += solveExpression2(expression);

            return sum;
        }

        private ulong solveExpression2(string expression)
        {
            Console.WriteLine($"exp: {expression}");

            if (expression.StartsWith("("))
            {
                var pIndex = expression.IndexOf("(", StringComparison.Ordinal);
                if (pIndex != -1)
                {
                    var pre = expression.Substring(0, pIndex);
                    var parExpression = grabParantheses(expression.Substring(pIndex));
                    var post = expression.Substring(pIndex + parExpression.Length);

                    parExpression = removeParantheses(parExpression);
                    parExpression = solveExpression2(parExpression).ToString();

                    expression = pre + parExpression + post;

                    return solveExpression2(expression);
                }

                expression = handleAddBeforeMult(expression);

                var left = grabParantheses(expression);
                var op = expression[left.Length + 1];
                var remaining = expression.Substring(left.Length + 3);

                left = removeParantheses(left);
                left = solveExpression2(left).ToString();

                if (char.IsDigit(remaining[0]))
                    return handleSecondTermIsDigit2(remaining, left, op);

                if (remaining.StartsWith("("))
                    return handleSecondTermIsParantheses2(remaining, left, op);

                throw new NotImplementedException();
            }

            if (char.IsDigit(expression[0]))
            {
                var pIndex = expression.IndexOf("(", StringComparison.Ordinal);
                if (pIndex != -1)
                {
                    var pre = expression.Substring(0, pIndex);
                    var parExpression = grabParantheses(expression.Substring(pIndex));
                    var post = expression.Substring(pIndex + parExpression.Length);

                    parExpression = removeParantheses(parExpression);
                    parExpression = solveExpression2(parExpression).ToString();

                    expression = pre + parExpression + post;

                    return solveExpression2(expression);
                }

                expression = handleAddBeforeMult(expression);

                var spaceIndex = expression.IndexOf(" ", StringComparison.Ordinal);
                if (spaceIndex == -1) return calcExpression(expression, "1", '*');

                var left = expression.Substring(0, spaceIndex);
                var op = expression[spaceIndex + 1];
                var remaining = expression.Substring(spaceIndex + 3); // +3 for space + op + space

                if (char.IsDigit(remaining[0]))
                    return handleSecondTermIsDigit2(remaining, left, op);

                if (remaining.StartsWith("("))
                    return handleSecondTermIsParantheses2(remaining, left, op);
            }

            throw new NotImplementedException();
        }

        private string handleAddBeforeMult(string expression)
        {
            var addRegex = new Regex(@"(?<left>\d+) \+ (?<right>\d+)");
            var match = addRegex.Match(expression);

            while (match.Success)
            {
                var res = calcExpression(match.Groups["left"].Value, match.Groups["right"].Value, '+');

                expression = expression.Substring(0, match.Index) + res +
                             expression.Substring(match.Index + match.Length);

                match = addRegex.Match(expression);
            }

            return expression;
        }

        private ulong handleSecondTermIsDigit2(string remaining, string left, char op)
        {
            var spaceIndex = remaining.IndexOf(" ", StringComparison.Ordinal);
            if (spaceIndex == -1) return calcExpression(left, remaining, op);

            var right = remaining.Substring(0, spaceIndex);
            remaining = remaining.Substring(spaceIndex);

            left = calcExpression(left, right, op).ToString();

            return solveExpression2(left + remaining);
        }

        private ulong handleSecondTermIsParantheses2(string remaining, string left, char op)
        {
            var right = grabParantheses(remaining);
            remaining = remaining.Substring(right.Length);

            right = removeParantheses(right);
            right = solveExpression2(right).ToString();

            return remaining == string.Empty
                ? calcExpression(left, right, op)
                : solveExpression2(calcExpression(left, right, op) + remaining);
        }
    }
}
