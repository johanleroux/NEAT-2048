using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public class Species
    {
        public Guid instanceID { get; private set; }

        public List<Genome> genomes;

        public Genome candidate;

        public double averageFitness, sumAdjustedFitness, extinctionCounter;

        public int populationSize;

        public Species()
        {
            this.instanceID = Guid.NewGuid();

            this.genomes = new List<Genome>();

            this.candidate = null;

            this.populationSize = 0;

            this.sumAdjustedFitness = 0f;

            this.extinctionCounter = 0f;
        }

        public void randomGenome()
        {
            this.genomes.Add(new Genome());
        }

        public List<Genome> getGenomes()
        {
            return this.genomes;
        }
    }
}
