using NEAT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NEAT.NEAT.Models
{
    public class Species
    {
        public Guid instanceID { get; private set; }

        private static int counter = 0;
        private readonly int id = counter++;

        public List<Genome> genomes;

        public Genome candidate { get; private set; }

        public double highestFitness { get; private set; } = 0;
        public int failedGenerations { get; private set; } = 0;

        public Species(ref Genome genome)
        {
            this.instanceID = Guid.NewGuid();

            this.genomes = new List<Genome>();

            this.candidate = genome;
            genome.setSpecies(this);
        }

        public int getID()
        {
            return this.id;
        }

        public void setHighestFitness(double value)
        {
            this.highestFitness = value;
            this.failedGenerations = 0;
        }

        public void setFailedGenerations(int value)
        {
            this.failedGenerations = value;
        }

        public List<Genome> getGenomes()
        {
            return this.genomes;
        }

        public bool isCompatible(ref Genome genome)
        {
            return Genome.distance(this.candidate, genome) <= Config.SPECIES_COMPATIBILTY_DISTANCE;
        }

        public void updateCandidate()
        {
            if (this.genomes.Count == 0) return;

            this.candidate = this.genomes[RandomUtil.integer(0, this.genomes.Count - 1)];
        }

        public void removeGenome(ref Genome g)
        {
            if (!this.genomes.Remove(g))
                throw new Exception("Unable to remove genome " + g.getID());
        }

        public double averageFitness()
        {
            double total = 0f;
            int counter = 0;
            foreach(Genome g in genomes)
            {
                total += g.getFitness();
                counter++;
            }
            return total / counter;
        }

        public List<Genome> bestGenomes()
        {
            List<Genome> best = new List<Genome>();
            foreach(Genome g in genomes)
                best.Add(g);

            foreach (Genome g in best)
                g.getFitness();

            //best.OrderBy(o => o.getFitness());
            return best;
        }
    }
}
