using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public class Synapse
    {
        public Guid instanceID { get; private set; }
        public Neuron from, to;

        public double weight;
        protected bool enabled;

        public long invovationID;

        public Synapse()
        {
            this.instanceID = Guid.NewGuid();

            this.from = null;
            this.to = null;

            this.weight = 0f;
            this.enabled = true;
        }

        public void disable()
        {
            this.enabled = false;
        }
    }
}
