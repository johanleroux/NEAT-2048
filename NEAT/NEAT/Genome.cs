using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    class Genome
    {
        protected List<Synapse> synapses;

        protected double fitness, adjustedFitness;

        public Genome()
        {
            this.synapses = new List<Synapse>();

            this.fitness = 0f;

            this.adjustedFitness = 0f;
        }
    }
}
