using NEAT.NEAT.Models;
using NEAT.Utils;
using System;
using System.Collections.Generic;

namespace NEAT.NEAT
{
    public class PopulationManager
    {
        private readonly NeatManager neat;

        public readonly Population currentPop;
        private int currentGeneration = 0;
        private int populationSize = 10;
        public Genome latestFitness;

        public PopulationManager(NeatManager neat)
        {
            this.neat = neat;
            currentPop = new Population(neat);
        }

        public List<Species> getSpecies()
        {
            return this.currentPop.species;
        }

        private Genome initial()
        {
            int[] inputs = new int[neat.inputSize];
            for (int i = 0; i < inputs.Length; i++)
                inputs[i] = i + 1;

            int[] outputs = new int[neat.outputSize];
            for (int i = 0; i < outputs.Length; i++)
                outputs[i] = i + 1;

            double dist = Config.MUTATION_WEIGHT_CHANCE_RANDOM_RANGE;
            Genome genome = new Genome(neat, null, inputs, outputs);
            for (int inNode = 1; inNode <= inputs.Length; inNode++)
            {
                for (int outNode = 1; outNode <= outputs.Length; outNode++)
                {
                    double weight = RandomUtil.doubleRand(-dist, dist);
                    genome.addSynapse(new Synapse(neat.getNextInnovationNumber(), inNode, neat.inputSize + outNode, weight, true), null, null);
                }
            }

            return genome;
        }

        public void initialize(int populationSize)
        {
            this.populationSize = populationSize;

            if (this.currentGeneration != 0)
                return;

            Genome init = this.initial();

            for (int i = 0; i < this.populationSize; i++)
            {
                Genome genome = init.clone();
                foreach (Synapse synapse in genome.getSynapses())
                {
                    double dist = Config.MUTATION_WEIGHT_CHANCE_RANDOM_RANGE;
                    synapse.weight = RandomUtil.doubleRand(-dist, dist);
                }
                this.currentPop.addGenome(ref genome);
            }
        }

        public void newGeneration()
        {
            this.currentGeneration++;

            // Get best genomes for every species
            Dictionary<Species, List<Genome>> bestGenomes = new Dictionary<Species, List<Genome>>();
            foreach(Species species in this.getSpecies())
                bestGenomes.Add(species, species.bestGenomes());

            Console.WriteLine("debug");
            return;

            // Total of the average fitness
            double sum = 0;
            foreach (Species species in this.getSpecies())
                sum += species.averageFitness();

            // VIPS Species
            Dictionary<Species, Genome> vips = new Dictionary<Species, Genome>();
            for(int s = 0; s < this.getSpecies().Count; s++)
            {
                Species vip = this.getSpecies()[s];

                // Remove worst genomes
                List<Genome> best = bestGenomes[vip];
                if (best.Count == 0) return;

                double remove = Math.Ceiling(best.Count * Config.GENERATION_ELIMINATION_PERCENTAGE);
                int start = (int)(Math.Floor(best.Count - remove) + 1);

                for (int i = start; i < best.Count; i++)
                {
                    Genome bad = best[i];
                    vip.removeGenome(ref bad);
                }

                // Remove species older than 20 generations
                vip.setFailedGenerations(vip.failedGenerations + 1);
                if(vip.failedGenerations > 20)
                {
                    Console.WriteLine("Species " + vip.getID() + " was removed for being older than 20 generations.");
                    this.getSpecies().RemoveAt(s);
                    continue;
                }

                // Remove species that too big
                double totalSize = this.populationSize;
                double breedsAllowed = Math.Floor(vip.averageFitness() / sum * totalSize) - 1.0;

                if (breedsAllowed < 1)
                {
                    Console.WriteLine("Species " + vip.getID() + " was removed for being too large");
                    this.getSpecies().RemoveAt(s);
                    continue;
                }

                // Copy the best genomes to the next generation
                Genome bestOfSpecies = best[0];
                vips.Add(vip, best[0]);
            }

            // Feedback
            int count = 0;
            foreach (Species species in this.getSpecies())
                count += species.genomes.Count;

            InfoManager.addLine("Building generation " + this.currentGeneration);
            InfoManager.addLine("Now " + this.getSpecies().Count + " species active (total of " + count + " genomes)");

            if(this.getSpecies().Count == 0)
            {
                InfoManager.addLine("All species have gone extinct");
                return;
            }

            int popSize = 0;

            // Add VIPS genomes
            Dictionary<Species, List<Genome>> oldGenomes = new Dictionary<Species, List<Genome>>();
            foreach(Species species in this.getSpecies())
            {
                oldGenomes.Add(species, species.genomes);

                species.genomes.Clear();

                Genome vip = null;
                if (vips.ContainsKey(species))
                    vip = vips[species];

                if(vip != null)
                {
                    species.genomes.Add(vip);
                    popSize++;
                }
            }

            // Fill the rest of the population with new children
            while (popSize < this.populationSize)
            {
                Species randomSpecies = this.getSpecies()[RandomUtil.integer(0, this.getSpecies().Count - 1)];
                List<Genome> oldMems = null;
                if (oldGenomes.ContainsKey(randomSpecies))
                    oldMems = oldGenomes[randomSpecies];

                if (oldMems != null && oldMems.Count > 0)
                {
                    if (RandomUtil.success(Config.BREED_CROSS_CHANCE))
                    {
                        // cross
                        Genome father = oldMems[RandomUtil.integer(0, oldMems.Count - 1)];
                        Genome mother = oldMems[RandomUtil.integer(0, oldMems.Count - 1)];

                        Genome g = Genome.cross(neat, father, mother);
                        this.currentPop.addGenome(ref g);
                    }
                    else
                    {
                        // don't cross just copy
                        Genome g = oldMems[RandomUtil.integer(0, oldMems.Count - 1)].clone();
                        g.mutate();
                        randomSpecies.genomes.Add(g);
                    }
                    popSize++;
                }
            }

            // Remove empty species
            for (int i = 0; i < this.getSpecies().Count; i++)
            {
                Species species = this.getSpecies()[i];
                if (species.genomes.Count == 0)
                    this.getSpecies().RemoveAt(i);
            }

            // Update species candidate genome
            foreach(Species species in this.getSpecies())
            {
                species.updateCandidate();
            }

            // Display performance
            this.latestFitness = this.currentPop.getBestPerforming();

            Console.WriteLine("Best performing genome [" + this.latestFitness.getID() + "] had fitness of " + this.latestFitness.getFitness() + " and was part of species " + this.latestFitness.species.getID() + " which has " + this.latestFitness.species.genomes.Count + " genomes");
            Console.WriteLine(this.latestFitness.toString());

            InfoManager.clearLine();
        }
    }
}