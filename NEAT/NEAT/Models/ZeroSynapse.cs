namespace NEAT.NEAT.Models
{
    public class ZeroSynapse
    {
        public readonly int from;
        public readonly int to;

        public ZeroSynapse(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
