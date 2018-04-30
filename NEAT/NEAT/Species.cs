using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    class Species
    {
        protected List<Genome> genomes;

        protected Genome candidate;

        protected double populationSize, sumAdjustedFitness, extinctionCounter;

        public Species()
        {
            this.genomes = new List<Genome>();

            this.candidate = null;

            this.populationSize = 0f;

            this.sumAdjustedFitness = 0f;

            this.extinctionCounter = 0f;
        }
    }
}
