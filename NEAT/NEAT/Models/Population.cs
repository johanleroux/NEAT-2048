using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT.Models
{
    public class Population
    {
        private readonly NeatManager neat;
        public readonly List<Species> species = new List<Species>();

        public Population(NeatManager neat)
        {
            this.neat = neat;
        }

        public void addGenome(ref Genome genome)
        {
            Species species = this.classify(ref genome);
            species.genomes.Add(genome);
        }

        private Species classify(ref Genome genome)
        {
            foreach (Species existing in this.species)
            {
                if (existing.isCompatible(ref genome))
                {
                    genome.setSpecies(existing);
                    return existing;
                }
            }

            Species ge = new Species(ref genome);
            this.species.Add(ge);

            return ge;
        }

        public Genome getBestPerforming()
        {
            Genome best = null;
            double bestFitness = -1;

            foreach (Species species in this.species)
            {
                foreach (Genome genome in species.genomes)
                {
                    if (best == null || genome.getFitness() > bestFitness)
                    {
                        best = genome;
                        bestFitness = genome.getFitness();
                    }
                }
            }

            return best;
        }
    }
}
