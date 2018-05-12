using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public static class Fitness
    {
        public static void deepFitness(Species species)
        {
            InfoManager.addLine("Calculating fitness for species " + species.instanceID);

            for(int i = 0; i < species.genomes.Count; i++)
            {
                InfoManager.addLine("Genome: " + species.genomes[i].instanceID);

                species.genomes[i].fitness = Fitness.calculateFitness(species.genomes[i]);

                InfoManager.addLine("Fitness: " + species.genomes[i].fitness);
            }
        }

        public static int calculateFitness(Genome genome)
        {
            Random r = new Random();

            return r.Next(1, 5000);
        }

        public static int calculateAdjustedFitness(Species species)
        {
            int adjustedFitness = 0;

            for (int i = 0; i < species.genomes.Count; i++)
            {
                species.genomes[i].fitness = Fitness.calculateFitness(species.genomes[i]);

                InfoManager.addLine("Fitness: " + species.genomes[i].fitness);
            }

            return adjustedFitness;
        }

        internal static void fittest(Species species, int totalFitness)
        {
            species.genomes.OrderBy(o => o.fitness);

            int weakCount = 0;

            if (species.genomes.Count > 2)
                weakCount = species.genomes.Count;

            InfoManager.addLine("Species " + species.instanceID + " generates " + 0 + " fitness with " + species.genomes.Count + " genomes");

            int survivalCount = species.genomes.Count - weakCount;

            species.populationSize = species.genomes.Count;

            species.populationSize = species.populationSize + (int) Math.Floor((species.sumAdjustedFitness / totalFitness) * Config.BABIES_PER_GENERATION);

            while(species.genomes.Count > survivalCount)
            {
                species.genomes.Remove(species.genomes[0]);
            }

            InfoManager.addLine("Genomes left in species " + species.instanceID + ": " + species.genomes.Count);
        }
    }
}
