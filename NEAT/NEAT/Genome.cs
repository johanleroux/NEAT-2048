using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public class Genome
    {
        private const double MUTATION_WEIGHT_CHANGE_PERTURBED_CHANCE = 0.2;

        public Guid instanceID { get; private set; }

        public List<Synapse> synapses;

        public List<Neuron> neurons;

        public double fitness, adjustedFitness;

        private Random r;

        public Genome()
        {
            this.instanceID = Guid.NewGuid();

            this.r = new Random();

            this.synapses = new List<Synapse>();

            this.neurons = new List<Neuron>();

            this.fitness = 0f;

            this.adjustedFitness = 0f;
        }

        public void mutate()
        {
            for(int i = 0; i < synapses.Count; i++)
            {
                if (r.NextDouble() < MUTATION_WEIGHT_CHANGE_PERTURBED_CHANCE)
                {
                    // mutate a little bit (perturbed)
                    synapses[i].weight = synapses[i].weight + r.NextDouble();
                }
                else
                {
                    // completely random
                    synapses[i].weight = r.NextDouble();
                }
            }
        }

        public void addSynapse()
        {
            // Fetch two neurons not connected
            Neuron from = getRandomNeuron(null, true);
            Neuron to = getRandomNeuron(from.instanceID, false);

            // Add synapse between the neurons
            Synapse synapse = new Synapse();
            synapse.from = from;
            synapse.to = to;

            // with a random weight
            synapse.weight = r.NextDouble();

            this.synapses.Add(synapse);
        }

        public void addNeuron()
        {
            Synapse oldSynapse = this.synapses[r.Next(0, this.synapses.Count)];

            Synapse fromSynapse = new Synapse();
            Synapse toSynapse = new Synapse();

            Neuron neuron = new Neuron();

            // Disable old synapse
            oldSynapse.disable();

            // Setup from synapse
            fromSynapse.from = oldSynapse.from;
            fromSynapse.to = neuron;
            fromSynapse.weight = 1;

            // Setup to synapse
            toSynapse.from = neuron;
            toSynapse.to = oldSynapse.to;
            toSynapse.weight = oldSynapse.weight;
        }

        public Neuron getRandomNeuron(Guid? previous, bool input = false)
        {
            Random r = new Random();
            Neuron neuron = this.neurons[r.Next(0, this.neurons.Count)];

            if (!input && neuron.type == NeuronType.input)
                neuron = getRandomNeuron(previous, input);

            if (neuron.instanceID == previous)
                neuron = getRandomNeuron(previous, input);

            return neuron;
        }
    }
}
