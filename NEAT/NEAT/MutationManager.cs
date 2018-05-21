using NEAT.NEAT.Models;
using NEAT.Utils;
using System;
using System.Collections.Generic;

namespace NEAT.NEAT
{
    public class MutationManager
    {
        private Genome genome;

        public MutationManager(Genome genome)
        {
            this.genome = genome;
        }

        public void mutate()
        {
            if (RandomUtil.success(Config.MUTATION_CHANCE_NEW_NEURON))
                addNewNeuron();

            if (RandomUtil.success(Config.MUTATION_CHANCE_NEW_SYNAPSE))
                addNewSynapse();

            if (RandomUtil.success(Config.MUTATION_CHANCE_MUTATE_SYNAPSE))
                mutateSynapses();
        }

        /**
         * Split up a current synapse
         * Add a new neuron between the two synapses
         * Synapse into new neuron has a weight of 1
         * Synapse going out from the new neuron has a weight of the previous connection
         */
        private void addNewNeuron()
        {
            Synapse randomSynapse = this.genome.getSynapses()[RandomUtil.integer(0, this.genome.getSynapses().Count - 1)];
            randomSynapse.enabled = true;

            int from = randomSynapse.from;
            int to = randomSynapse.to;
            this.genome.neat.getNextInnovationNumber();

            int newNeuronId = (this.genome.getHighestNeuron() + 1);
            this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), from, newNeuronId, 1, true), null, null);
            this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), newNeuronId, to, randomSynapse.weight, true), null, null);
        }

        /**
         * Add a synapse between two random neurons
         * not currently connected
         * add a random weight to the synapse
         */
        private void addNewSynapse()
        {
            List<ZeroSynapse> currentSynapses = this.genome.getAllSynapses();
            int attempts = 0;
            ZeroSynapse possibleSynapse = null;

            do
            {
                if (attempts++ > 40) return;

                int from = genome.getNeurons(true, true, false)[RandomUtil.integer(0, this.genome.getNeurons(true, true, false).Count - 1)];
                List<int> leftOver = this.genome.getNeurons(false, true, true);

                if (leftOver.Count == 0) continue;

                int to = leftOver[RandomUtil.integer(0, leftOver.Count - 1)];
                possibleSynapse = new ZeroSynapse(from, to);
            } while (((possibleSynapse == null) || ((possibleSynapse.from == possibleSynapse.to) || (currentSynapses.Contains(possibleSynapse) || isRecurrent(possibleSynapse)))));

            genome.addSynapse(new Synapse(genome.neat.getNextInnovationNumber(), possibleSynapse.from, possibleSynapse.to, RandomUtil.integer(-1, 1), true), null, null);
        }

        /*
         * Mutate current synapse weights
         */
        private void mutateSynapses()
        {
            if (RandomUtil.success(Config.SYNAPSE_WEIGHT_RANDOM_CHANCE))
            {
                foreach (Synapse synapse in this.genome.getSynapses())
                {
                    double range = Config.SYNAPSE_WEIGHT_RANGE;
                    synapse.weight = RandomUtil.doubleRand((range * -1), range);
                }
            }
            else
            {
                foreach (Synapse synapse in this.genome.getSynapses())
                {
                    double disturbance = Config.SYNAPSE_WEIGHT_PERTRUDE;
                    double uniform = RandomUtil.doubleRand((disturbance * -1), disturbance);
                    synapse.weight = synapse.weight + uniform;
                }

            }
        }

        // REF: [GITHUB] SanderGielisse logic for looping
        // and testing unconnected neurons
        // (https://github.com/SanderGielisse/Mythan)
        private bool isRecurrent(ZeroSynapse with)
        {
            Genome tmpGenome = this.genome.clone();

            if ((with != null))
            {
                Synapse synapse = new Synapse((tmpGenome.getHighestInnovationNumber() + 1), with.from, with.to, 0, true);
                tmpGenome.addSynapse(synapse, null, null);
            }

            bool recc = false;
            foreach (int hiddenNeuron in tmpGenome.getHiddenNeurons())
                if (this.isRecurrent(new List<int>(), ref tmpGenome, hiddenNeuron))
                    recc = true;

            return recc;
        }

        private bool isRecurrent(List<int> path, ref Genome genome, int neuron)
        {
            if (path.Contains(neuron)) return true;

            path.Add(neuron);
            bool recc = false;
            foreach (int from in getInputs(ref this.genome, neuron))
                if (!this.genome.isInputNeuron(from))
                    if (this.isRecurrent(path, ref this.genome, from))
                        recc = true;

            return recc;
        }

        private List<int> getInputs(ref Genome genome, int neuron)
        {
            List<int> froms = new List<int>();
            foreach (Synapse synapse in this.genome.getSynapses())
                if ((synapse.to == neuron))
                    froms.Add(synapse.from);

            return froms;
        }
    }
}