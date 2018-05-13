using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Utils
{
    static class RandomUtil
    {
        private static readonly Random r = new Random();
        private static readonly object synLock = new object();

        public static int integer()
        {
            lock(synLock)
                return r.Next();
        }
        public static int integer(int min, int max)
        {
            lock (synLock)
                return r.Next(min, max);
        }

        public static bool success(double chance)
        {
            lock (synLock)
                return r.NextDouble() <= chance;
        }

        internal static double doubleRand(double min, double max)
        {
            lock (synLock)
                return r.NextDouble() * (max - (-min) + (-min));
        }
    }
}
