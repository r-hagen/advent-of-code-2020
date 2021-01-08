using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode.Puzzles
{
    public static class ComboBreaker
    {
        public static string Solve1(int cardPublicKey, int doorPublicKey)
        {
            var cardLoopSize = findLoopSize(cardPublicKey);
            var doorLoopSize = findLoopSize(doorPublicKey);

            var cardEncryptionKey = transformSubjectNumber(doorPublicKey, cardLoopSize);
            var doorEncryptionKey = transformSubjectNumber(cardPublicKey, doorLoopSize);

            Debug.Assert(cardEncryptionKey == doorEncryptionKey);

            return cardEncryptionKey.ToString();
        }

        private static int findLoopSize(int publicKey)
        {
            const int subjectNumber = 7;
            var transformed = 1;
            var loopSize = 0;

            do
            {
                transformed = checked(transformed * subjectNumber);
                transformed %= 20201227;
                loopSize++;
            } while (transformed != publicKey);

            return loopSize;
        }

        private static BigInteger transformSubjectNumber(int subjectNumber, int loopSize)
        {
            BigInteger transformed = 1;

            for (var i = 0; i < loopSize; i++)
            {
                transformed *= subjectNumber;
                transformed %= 20201227;
            }

            return transformed;
        }
    }
}