using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    class Neuron
    {
        public Guid instanceID { get; private set; }

        public Neuron()
        {
            this.instanceID = Guid.NewGuid();
        }
    }
}
