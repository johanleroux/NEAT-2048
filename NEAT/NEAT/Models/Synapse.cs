using System;

namespace NEAT.NEAT.Models
{
    public class Synapse
    {
        public Guid instanceID { get; private set; }

        public int innovationNumber;

        public readonly int from;
        public readonly int to;

        public double weight;
        public bool enabled;

        public Synapse(int innovationNumber, int from, int to, double weight, bool enabled)
        {
            this.instanceID = Guid.NewGuid();

            this.innovationNumber = innovationNumber;
            this.from = from;
            this.to = to;
            this.weight = weight;
            this.enabled = enabled;
        }

        public Synapse clone()
        {
            return new Synapse(this.innovationNumber, this.from, this.to, this.weight, this.enabled);
        }

        public void disable()
        {
            this.enabled = false;
        }

        public String toString()
        {
            return "Synapse"
                + "[innovationNumber=" + this.innovationNumber
                + ", from=" + this.from
                + ", to=" + this.to
                + ", weight=" + this.weight
                + ", enabled=" + this.enabled
                + "]";
        }
    }
}
