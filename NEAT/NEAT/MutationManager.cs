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
            /**
             * 1. Add a new node. The new input weight to that node will be 1.
             * 	  The output from the new node will be set to the old connection's weight value.
             */
            if (RandomUtil.success(Config.MUTATION_NEW_NODE_CHANCE))
            {
                Synapse randomSynapse = this.genome.getSynapses()[RandomUtil.integer(0, this.genome.getSynapses().Count - 1)];
                randomSynapse.enabled = true;

                //  two new genes
                int from = randomSynapse.from;
                int to = randomSynapse.to;
                this.genome.neat.getNextInnovationNumber();
                int newNodeId = (this.genome.getHighestNode() + 1);
                this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), from, newNodeId, 1, true), null, null);
                this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), newNodeId, to, randomSynapse.weight, true), null, null);
            }

            /**
             * 2. Add a new link with a random weight between two existing nodes.
             *    Start by finding two yet unconnected nodes. One of them must be a hidden node.
             */
            if (RandomUtil.success(Config.MUTATION_NEW_CONNECTION_CHANCE))
            {
                List<Connection> currentConnections = this.genome.getAllConnections();
                int attempts = 0;
                Connection maybeNew = null;

                do
                {
                    if (attempts++ > 40) return;

                    int from = this.genome.getNodes(true, true, false)[RandomUtil.integer(0, this.genome.getNodes(true, true, false).Count - 1)];
                    List<int> leftOver = this.genome.getNodes(false, true, true);
                    if (!leftOver.Remove(from))
                        throw new Exception("Unable to remove genome");

                    if (leftOver.Count == 0) continue;

                    int to = leftOver[RandomUtil.integer(0, leftOver.Count - 1)];
                    maybeNew = new Connection(from, to);
                } while (((maybeNew == null) || ((maybeNew.from == maybeNew.to) || (currentConnections.Contains(maybeNew) || this.isRecurrent(maybeNew)))));

                this.genome.addSynapse(new Synapse(this.genome.neat.getNextInnovationNumber(), maybeNew.from, maybeNew.to, RandomUtil.integer(-1, 1), true), null, null);
            }

            /**
             * 3. The weights of an existing connection are changed.
             */
            if (RandomUtil.success(Config.MUTATION_WEIGHT_CHANCE))
            {
                if (RandomUtil.success(Config.MUTATION_WEIGHT_RANDOM_CHANCE))
                {
                    //  assign a random new value
                    foreach (Synapse synapse in this.genome.getSynapses())
                    {
                        double range = Config.MUTATION_WEIGHT_CHANCE_RANDOM_RANGE;
                        synapse.weight = RandomUtil.doubleRand((range * -1), range);
                    }
                }
                else
                {
                    //  uniformly perturb
                    foreach (Synapse synapse in this.genome.getSynapses())
                    {
                        double disturbance = Config.MUTATION_WEIGHT_MAX_DISTURBANCE;
                        double uniform = RandomUtil.doubleRand((disturbance * -1), disturbance);
                        synapse.weight = synapse.weight + uniform;
                    }

                }
            }
        }

        public bool isRecurrent(Connection with)
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