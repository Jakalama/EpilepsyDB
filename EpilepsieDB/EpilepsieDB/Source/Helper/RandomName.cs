using System;
using System.Linq;

namespace EpilepsieDB.Source.Helper
{
    public static class RandomName
    {
        public static string Generate(int length = 10)
        {
            Random random = new Random();

            if (length < 10)
                length = 10;

            int numDigits = (int) Math.Floor(length / 3f);
            int numChars = (int) Math.Ceiling(length / 3f);
            int numAbst = length - numDigits - numChars;

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string nums = "0123456789";
            const string abst = "!§$%*+#-/:_";
            string part1 = new string(Enumerable.Repeat(chars, numChars)
                .Select(l => l[random.Next(l.Length)]).ToArray());
            string part2 = new string(Enumerable.Repeat(nums, numDigits)
                .Select(l => l[random.Next(l.Length)]).ToArray());
            string part3 = new string(Enumerable.Repeat(abst, numAbst)
                .Select(l => l[random.Next(l.Length)]).ToArray());

            return Shuffle(part1 + part2 + part3);
        }

        private static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
    }
}
