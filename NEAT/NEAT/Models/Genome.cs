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
        private List<int> inputNeurons = new List<int>();
        private List<int> outputNeurons = new List<int>();

        private double fitness = -1;

        public Genome(NeatManager neat, Species species, int[] inputNeurons, int[] outputNeurons)
        {
            this.instanceID = Guid.NewGuid();
            this.neat = neat;

            this.species = species;


            foreach (int inNeuron in inputNeurons)
                this.addInputNeuron(inNeuron);

            foreach (int outNeuron in outputNeurons)
                this.addOutputNeuron(outNeuron);
        }

        public Genome clone()
        {
            Genome genome = new Genome(this.neat, this.species, this.inputNeurons.ToArray(), this.outputNeurons.ToArray());

            // clone the values of the synapses
            genome.synapses = new Dictionary<int, Synapse>();
            foreach (KeyValuePair<int, Synapse> synapse in this.synapses)
                genome.synapses.Add(synapse.Key, synapse.Value);

            genome.inputNeurons = new List<int>(this.inputNeurons);
            genome.outputNeurons = new List<int>(this.outputNeurons);

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

        public void addInputNeuron(int neuron)
        {
            if (this.fitness != -1)
                throw new Exception("Training already begun");

            if (this.inputNeurons.Contains(neuron))
                return;

            this.inputNeurons.Add(neuron);
        }

        public void addOutputNeuron(int neuron)
        {
            if (this.fitness != -1)
                throw new Exception("Training already begun");

            if (this.outputNeurons.Contains(neuron))
                return;

            this.outputNeurons.Add(neuron);
        }

        public int[] getInputs()
        {
            return this.inputNeurons.ToArray();
        }

        public int[] getOutputs()
        {
            return this.outputNeurons.ToArray();
        }

        public List<int> getNeurons(bool input, bool hidden, bool output)
        {
            List<int> neurons = new List<int>();

            foreach(int neuron in getAllNeurons())
            {
                if (this.isInputNeuron(neuron) && !input)
                    continue;
                if (this.isHiddenNeuron(neuron) && !hidden)
                    continue;
                if (this.isOutputNeuron(neuron) && !output)
                    continue;

                neurons.Add(neuron);
            }

            return neurons;
        }

        public List<int> getAllNeurons()
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

        public List<int> getHiddenNeurons()
        {
            List<int> neurons = new List<int>();
            foreach (int neuron in getAllNeurons())
            {
                if (isHiddenNeuron(neuron))
                    neurons.Add(neuron);
            }
            return neurons;
        }

        public bool isInputNeuron(int neuron)
        {
            return this.inputNeurons.Contains(neuron);
        }

        public bool isOutputNeuron(int neuron)
        {
            return this.outputNeurons.Contains(neuron);
        }

        public bool isHiddenNeuron(int neuron)
        {
            return !this.isInputNeuron(neuron) && !this.isOutputNeuron(neuron);
        }

        public int getHighestNeuron()
        {
            List<int> neurons = this.getAllNeurons();
            return neurons[neurons.Count - 1];
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
                    bool dis1 = !parent1.synapses[synapse.innovationNumber].enabled;
                    bool dis2 = !parent2.synapses[synapse.innovationNumber].enabled;

                    if ((dis1 && !dis2) || (!dis1 && dis2))
                    {
                        bool disabled = RandomUtil.success(Config.NEURON_DISABLE_CHANCE);
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

        // REF: [GITHUB] SanderGielisse logic for distance
        // between genomes (eg compatibility)
        // (https://github.com/SanderGielisse/Mythan)
        public static double distance(Genome a, Genome b)
        {
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

            double disjoint = 0;
            double excess = 0;

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

            int similarityNeurons = -1;
            for (int i = 1; i < 100000; i++)
            {
                if (dominant.hasSynapse(i) && other.hasSynapse(i))
                    similarityNeurons = i;
            }

            if (similarityNeurons == -1)
                throw new Exception("No similarity between Genomes");

            Genome newGenome = new Genome(neat, null, dominant.inputNeurons.ToArray(), dominant.outputNeurons.ToArray());
            for (int i = 1; i <= dominant.getHighestInnovationNumber(); i++)
            {
                if (dominant.hasSynapse(i))
                {
                    int innovationNumber, inputNeuron, outputNeuron;
                    double weigth;
                    bool enabled;

                    if (other.hasSynapse(i))
                    {
                        Random r = new Random();

                        if (r.Next(0, 1) == 1)
                        {
                            innovationNumber = dominant.synapses[i].innovationNumber;
                            inputNeuron = dominant.synapses[i].from;
                            outputNeuron = dominant.synapses[i].to;
                            weigth = dominant.synapses[i].weight;
                            enabled = dominant.synapses[i].enabled;
                        }
                        else
                        {
                            innovationNumber = other.synapses[i].innovationNumber;
                            inputNeuron = other.synapses[i].from;
                            outputNeuron = other.synapses[i].to;
                            weigth = other.synapses[i].weight;
                            enabled = other.synapses[i].enabled;
                        }
                    }
                    else
                    {
                        innovationNumber = dominant.synapses[i].innovationNumber;
                        inputNeuron = dominant.synapses[i].from;
                        outputNeuron = dominant.synapses[i].to;
                        weigth = dominant.synapses[i].weight;
                        enabled = dominant.synapses[i].enabled;
                    }
                    newGenome.addSynapse(new Synapse(innovationNumber, inputNeuron, outputNeuron, weigth, enabled), dominant, other);
                }
            }

            newGenome.removeDuplicateSynapses();
            newGenome.mutate();

            return newGenome;
        }

        public List<ZeroSynapse> getAllSynapses()
        {
            List<ZeroSynapse> links = new List<ZeroSynapse>();
            foreach(Synapse synapse in this.getSynapses())
                links.Add(new ZeroSynapse(synapse.from, synapse.to));

            return links;
        }

        public List<ZeroSynapse> getActiveSynapses()
        {
            List<ZeroSynapse> links = new List<ZeroSynapse>();
            foreach (Synapse synapse in this.getSynapses())
                if(synapse.enabled)
                    links.Add(new ZeroSynapse(synapse.from, synapse.to));

            return links;
        }

        public void removeDuplicateSynapses()
        {
            if (this.fitness != -1)
                throw new Exception("Can't remove duplicate of a untested Genome");

            foreach (Species species in this.neat.popManager.currentPop.species)
            {
                foreach (Genome genome in species.genomes)
                {
                    List<ZeroSynapse> conA = this.getAllSynapses();
                    List<ZeroSynapse> conB = genome.getAllSynapses();

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
