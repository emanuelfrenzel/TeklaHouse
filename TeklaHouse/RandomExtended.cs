using System;

namespace TeklaHouse
{
    class RandomExtended
    {
        private static readonly Random random = new Random();

        public static int getRandom(int min, int max)
        {
            return Convert.ToInt32(random.Next(min, max));
        }
    }
}
