using NEAT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NEAT.NEAT.Models
{
    public class Genome
    {
        public Guid instanceID { get; private set; }
        public NeatManager neat { get; private set; }

        private static int counter = 0;
        private readonly int id = counter++;

        public Species species { get; private set; }

        private Dictionary<int, Synapse> synapses = new Dictionary<int, Synapse>();
        private List<int> inputNodes = new List<int>();
        private List<int> outputNodes = new List<int>();

        private double fitness = -1;

        public Genome(NeatManager neat, Species species, int[] inputNodes, int[] outputNodes)
        {
            this.instanceID = Guid.NewGuid();
            this.neat = neat;

            this.species = species;


            foreach (int inNode in inputNodes)
                this.addInputNode(inNode);

            foreach (int outNode in outputNodes)
                this.addOutputNode(outNode);
        }

        public Genome clone()
        {
            Genome genome = new Genome(this.neat, this.species, this.inputNodes.ToArray(), this.outputNodes.ToArray());

            // clone the values of the synapses
            genome.synapses = new Dictionary<int, Synapse>();
            foreach (KeyValuePair<int, Synapse> synapse in this.synapses)
                genome.synapses.Add(synapse.Key, synapse.Value);

            genome.inputNodes = new List<int>(this.inputNodes);
            genome.outputNodes = new List<int>(this.outputNodes);

            return genome;
        }

        public int getID()
        {
            return this.id;
        }

        public void setSpecies(Species species)
        {
            if (this.fitness != -1)
                return;

            this.species = species;
        }

        public void addInputNode(int node)
        {
            if (this.fitness != -1)
                throw new Exception("Training already begun");

            if (this.inputNodes.Contains(node))
                return;

            this.inputNodes.Add(node);
        }

        public void addOutputNode(int node)
        {
            if (this.fitness != -1)
                throw new Exception("Training already begun");

            if (this.outputNodes.Contains(node))
                return;

            this.outputNodes.Add(node);
        }

        public int[] getInputs()
        {
            return this.inputNodes.ToArray();
        }

        public int[] getOutputs()
        {
            return this.outputNodes.ToArray();
        }

        public List<int> getNodes(bool input, bool hidden, bool output)
        {
            List<int> nodes = new List<int>();

            foreach(int node in getAllNodes())
            {
                if (this.isInputNode(node) && !input)
                    continue;
                if (this.isHiddenNode(node) && !hidden)
                    continue;
                if (this.isOutputNode(node) && !output)
                    continue;

                nodes.Add(node);
            }

            return nodes;
        }

        public List<int> getAllNodes()
        {
            List<int> ids = new List<int>();
            foreach (Synapse synapse in synapses.Values)
            {
                if (!ids.Contains(synapse.from))
                {
                    ids.Add(synapse.from);
                }
                if (!ids.Contains(synapse.to))
                {
                    ids.Add(synapse.to);
                }
            }

            return ids.OrderBy(o => o).ToList();
        }

        public List<int> getHiddenNodes()
        {
            List<int> nodes = new List<int>();
            foreach (int node in getAllNodes())
            {
                if (isHiddenNode(node))
                    nodes.Add(node);
            }
            return nodes;
        }

        public bool isInputNode(int node)
        {
            return this.inputNodes.Contains(node);
        }

        public bool isOutputNode(int node)
        {
            return this.outputNodes.Contains(node);
        }

        public bool isHiddenNode(int node)
        {
            return !this.isInputNode(node) && !this.isOutputNode(node);
        }

        public int getHighestNode()
        {
            List<int> nodes = this.getAllNodes();
            return nodes[nodes.Count - 1];
        }

        public int getHighestInnovationNumber()
        {
            if (this.synapses.Count == 0) return -1;

            KeyValuePair<int, Synapse> synapse = synapses.Last();

            return synapse.Value.innovationNumber;
        }

        public void addSynapse(Synapse synapse, Genome parent1, Genome parent2)
        {
            if (this.fitness != -1)
                return;

            if (this.synapses.ContainsKey(synapse.innovationNumber))
                return;

            synapse = synapse.clone();

            if(parent1 != null && parent2 != null)
            {
                if (parent1.hasSynapse(synapse.innovationNumber) && parent2.hasSynapse(synapse.innovationNumber))
                {
                    /**
                     * There is a chance that a gene which is disabled in one of the parents is disabled.
                     */
                    bool dis1 = !parent1.synapses[synapse.innovationNumber].enabled;
                    bool dis2 = !parent2.synapses[synapse.innovationNumber].enabled;

                    // only one of them is disabled
                    if ((dis1 && !dis2) || (!dis1 && dis2))
                    {
                        bool disabled = RandomUtil.success(Config.GENE_DISABLE_CHANCE);
                        synapse.enabled = !disabled;
                    }
                }
            }

            this.synapses.Add(synapse.innovationNumber, synapse);
        }

        private bool hasSynapse(int innovationNumber)
        {
            return this.synapses.ContainsKey(innovationNumber);
        }

        private Synapse getSynapse(int innovationNumber)
        {
            if(this.synapses.ContainsKey(innovationNumber))
                return this.synapses[innovationNumber];
            return null;
        }

        public List<Synapse> getSynapses()
        {
            List<Synapse> tmp = new List<Synapse>();
            foreach(KeyValuePair<int, Synapse> synapse in synapses)
            {
                tmp.Add(synapse.Value);
            }
            return tmp;
        }

        public double getFitness()
        {
            if (this.fitness == -1)
                return calculateFitness();

            return this.fitness;
        }

        private double calculateFitness()
        {
            this.fitness = neat.runGenome(this);

            if (this.fitness > this.species.highestFitness)
            {
                this.species.setHighestFitness(this.fitness);
            }
            return this.fitness;
        }

        public static double distance(Genome a, Genome b)
        {
            // find the longest
            int aLength = a.getHighestInnovationNumber();
            int bLength = b.getHighestInnovationNumber();

            Genome longest;
            Genome shortest;

            if (aLength > bLength)
            {
                longest = a;
                shortest = b;
            }
            else
            {
                longest = b;
                shortest = a;
            }

            int shortestLength = shortest.getHighestInnovationNumber();
            int longestLength = longest.getHighestInnovationNumber();

            double disjoint = 0; // use double so it won't be used as an int in the formula
            double excess = 0; // use double so it won't be used as an int in the formula

            List<double> weights = new List<double>();
            for (int i = 0; i < longestLength; i++)
            {
                Synapse aa = longest.getSynapse(i);
                Synapse bb = shortest.getSynapse(i);

                if ((aa == null && bb != null) || (aa != null && bb == null))
                {
                    // only present in one of them

                    if (i <= shortestLength)
                    {
                        disjoint++;
                    }
                    else if (i > shortestLength)
                    {
                        excess++;
                    }
                }
                if (aa != null && bb != null)
                {
                    // matching gene
                    double distance = Math.Abs(aa.weight - bb.weight);
                    weights.Add(distance);
                }
            }

            double total = 0;
            double size = 0;

            foreach(double w in weights)
            {
                total += w;
                size++;
            }

            double averageWeightDistance = total / size;
            double n = longest.synapses.Count;
            double c1 = Config.DISTANCE_EXCESS_WEIGHT;
            double c2 = Config.DISTANCE_DISJOINT_WEIGHT;
            double c3 = Config.DISTANCE_WEIGHTS_WEIGHT;

            // formula: d = (c1 * E) / N + (c2 * D) / N + c3 * W
            double d = ((c1 * excess) / n) + ((c2 * disjoint) / n) + (c3 * averageWeightDistance);
            return d;
        }
        public static Genome cross(NeatManager neat, Genome a, Genome b)
        {
            double aFitness = a.fitness;
            double bFitness = b.fitness;

            Genome strongest;
            Genome weakest;
            if (aFitness > bFitness)
            {
                strongest = a;
                weakest = b;
            }
            else
            {
                strongest = b;
                weakest = a;
            }

            return crossDominant(neat, strongest, weakest);
        }

        private static Genome crossDominant(NeatManager neat, Genome dominant, Genome other)
        {
            if (dominant.synapses.Count == 0 || other.synapses.Count == 0)
                return null;

            // find out how far they match
            int sharedLength = -1;
            for (int i = 1; ; i++)
            {
                if (i > 100000)
                    throw new Exception();

                if (dominant.hasSynapse(i) && other.hasSynapse(i))
                {
                    sharedLength = i;
                }
                else
                {
                    break;
                }
            }
            if (sharedLength == -1)
                throw new Exception();

            Genome newGenome = new Genome(neat, null, dominant.inputNodes.ToArray(), dominant.outputNodes.ToArray()); // inputs/outputs should match so it doesn't matter where we get it from
            for (int i = 1; i <= dominant.getHighestInnovationNumber(); i++)
            {
                if (dominant.hasSynapse(i))
                {
                    int innovationNumber, inputNode, outputNode;
                    double weigth;
                    bool enabled;

                    if (other.hasSynapse(i))
                    {
                        Random r = new Random();

                        if (r.Next(0, 1) == 1)
                        {
                            innovationNumber = dominant.synapses[i].innovationNumber;
                            inputNode = dominant.synapses[i].from;
                            outputNode = dominant.synapses[i].to;
                            weigth = dominant.synapses[i].weight;
                            enabled = dominant.synapses[i].enabled;
                        }
                        else
                        {
                            innovationNumber = other.synapses[i].innovationNumber;
                            inputNode = other.synapses[i].from;
                            outputNode = other.synapses[i].to;
                            weigth = other.synapses[i].weight;
                            enabled = other.synapses[i].enabled;
                        }
                    }
                    else
                    {
                        innovationNumber = dominant.synapses[i].innovationNumber;
                        inputNode = dominant.synapses[i].from;
                        outputNode = dominant.synapses[i].to;
                        weigth = dominant.synapses[i].weight;
                        enabled = dominant.synapses[i].enabled;
                    }
                    newGenome.addSynapse(new Synapse(innovationNumber, inputNode, outputNode, weigth, enabled), dominant, other);
                }
            }

            // make sure there are no duplicates
            newGenome.fixDuplicates();

            // do mutations
            newGenome.mutate();

            return newGenome;
        }

        public List<Connection> getAllConnections()
        {
            List<Connection> links = new List<Connection>();
            foreach(Synapse synapse in this.getSynapses())
                links.Add(new Connection(synapse.from, synapse.to));

            return links;
        }

        public List<Connection> getActiveConnections()
        {
            List<Connection> links = new List<Connection>();
            foreach (Synapse synapse in this.getSynapses())
                if(synapse.enabled)
                    links.Add(new Connection(synapse.from, synapse.to));

            return links;
        }

        public void fixDuplicates()
        {
            if (this.fitness != -1)
                return;

            foreach (Species species in this.neat.popManager.currentPop.species)
            {
                foreach (Genome genome in species.genomes)
                {
                    List<Connection> conA = this.getAllConnections();
                    List<Connection> conB = genome.getAllConnections();

                    if (ListUtil.equals(conB, conA))
                    {
                        List<Synapse> toCloneFrom = new List<Synapse>(genome.synapses.Values);
                        List<Synapse> toReplace = new List<Synapse>(this.synapses.Values);

                        for (int i = 0; i < toCloneFrom.Count; i++)
                        {
                            if (toCloneFrom[i] == null || toReplace[i] == null) return;

                            Synapse from = toCloneFrom[i];
                            Synapse to = toReplace[i];

                            int oldInno = to.innovationNumber;
                            int changeTo = from.innovationNumber;

                            Synapse old = this.synapses[oldInno];
                            if (!this.synapses.Remove(oldInno))
                                new Exception("Synapse could not be removed " + old.toString());
                            old.innovationNumber = changeTo;

                            this.synapses.Add(old.innovationNumber, old);
                        }
                    }
                }
            }
        }

        public void mutate()
        {
            MutationManager mutation = new MutationManager(this);
            mutation.mutate();
        }

        public double[] calculateMove(double[] inputs)
        {
            return new Backtrack(this, inputs).calculateOutput();
        }

        public String toString()
        {
            String tmp = "";

            for(int i = 0; i < getSynapses().Count; i++)
            {
                tmp += getSynapses()[i].toString();
                if(i < getSynapses().Count - 1)
                    tmp += Environment.NewLine;
            }

            return tmp;
        }
    }
}
