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
            if (RandomUtil.success(Config.MUTATION_NEW_NODE_CHANCE))
                addNewNode();

            if (RandomUtil.success(Config.MUTATION_NEW_CONNECTION_CHANCE))
                addNewSynapse();

            if (RandomUtil.success(Config.MUTATION_WEIGHT_CHANCE))
                mutateSynapses();
        }

        private void addNewNode()
        {
            Synapse randomSynapse = this.genome.getSynapses()[RandomUtil.integer(0, this.genome.getSynapses().Count - 1)];
            randomSynapse.enabled = true;

            int from = randomSynapse.from;
            int to = randomSynapse.to;
            this.genome.neat.getNextInnovationNumber();

            int newNodeId = (this.genome.getHighestNode() + 1);
            this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), from, newNodeId, 1, true), null, null);
            this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), newNodeId, to, randomSynapse.weight, true), null, null);
        }

        private void addNewSynapse()
        {
            List<Connection> currentConnections = this.genome.getAllConnections();
            int attempts = 0;
            Connection testConnection = null;

            do
            {
                if (attempts++ > 40) return;

                int from = genome.getNodes(true, true, false)[RandomUtil.integer(0, this.genome.getNodes(true, true, false).Count - 1)];
                List<int> leftOver = this.genome.getNodes(false, true, true);

                if (leftOver.Count == 0) continue;

                int to = leftOver[RandomUtil.integer(0, leftOver.Count - 1)];
                testConnection = new Connection(from, to);
            } while (((testConnection == null) || ((testConnection.from == testConnection.to) || (currentConnections.Contains(testConnection) || isRecurrent(testConnection)))));

            genome.addSynapse(new Synapse(genome.neat.getNextInnovationNumber(), testConnection.from, testConnection.to, RandomUtil.integer(-1, 1), true), null, null);
        }

        private void mutateSynapses()
        {
            if (RandomUtil.success(Config.MUTATION_WEIGHT_RANDOM_CHANCE))
            {
                foreach (Synapse synapse in this.genome.getSynapses())
                {
                    double range = Config.MUTATION_WEIGHT_CHANCE_RANDOM_RANGE;
                    synapse.weight = RandomUtil.doubleRand((range * -1), range);
                }
            }
            else
            {
                foreach (Synapse synapse in this.genome.getSynapses())
                {
                    double disturbance = Config.MUTATION_WEIGHT_MAX_DISTURBANCE;
                    double uniform = RandomUtil.doubleRand((disturbance * -1), disturbance);
                    synapse.weight = synapse.weight + uniform;
                }

            }
        }

        private bool isRecurrent(Connection with)
        {
            Genome tmpGenome = this.genome.clone();
            //  clone so we can change its genes without actually affecting the original genome
            if ((with != null))
            {
                Synapse synapse = new Synapse((tmpGenome.getHighestInnovationNumber() + 1), with.from, with.to, 0, true);
                tmpGenome.addSynapse(synapse, null, null);
            }

            bool recc = false;
            foreach (int hiddenNode in tmpGenome.getHiddenNodes())
                if (this.isRecurrent(new List<int>(), ref tmpGenome, hiddenNode))
                    recc = true;

            return recc;
        }

        private bool isRecurrent(List<int> path, ref Genome genome, int node)
        {
            if (path.Contains(node)) return true;

            path.Add(node);
            bool recc = false;
            foreach (int from in getInputs(ref this.genome, node))
                if (!this.genome.isInputNode(from))
                    if (this.isRecurrent(path, ref this.genome, from))
                        recc = true;

            return recc;
        }

        private List<int> getInputs(ref Genome genome, int node)
        {
            List<int> froms = new List<int>();
            foreach (Synapse synapse in this.genome.getSynapses())
                if ((synapse.to == node))
                    froms.Add(synapse.from);

            return froms;
        }
    }
}