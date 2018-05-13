using System;

namespace NEAT.NEAT.Models
{
    public class Connection
    {
        public readonly int from;
        public readonly int to;

        public Connection(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
