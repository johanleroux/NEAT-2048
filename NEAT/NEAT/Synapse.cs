using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    class Synapse
    {
        public Guid instanceID { get; private set; }
        protected Neuron from, to;

        protected double weight;
        protected bool enabled;

        public Synapse()
        {
            this.instanceID = Guid.NewGuid();

            this.from = null;
            this.to = null;

            this.weight = 0f;
            this.enabled = false;
        }
    }
}
