using NEAT.NEAT.Models;
using System.Collections.Generic;

namespace NEAT.Utils
{
    public static class ListUtil
    {
        public static bool equals(List<Connection> a, List<Connection> b)
        {
            if (a.Count != b.Count)
                return false;

            for(int i = 0; i < a.Count; i++)
                if (!a[i].Equals(b[i]))
                    return false;

            return true;
        }
    }
}
