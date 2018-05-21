using NEAT.NEAT.Models;
using System;
using System.Collections.Generic;

namespace NEAT.NEAT
{
    class Backtrack
    {
        private Genome genome;
        private Dictionary<int, double> neuronInputValues = new Dictionary<int, double>();

        public Backtrack(Genome genome, double[] inputs)
        {
            this.genome = genome;

            if (genome.getInputs().Length != inputs.Length)
                throw new Exception("Input size " + inputs.Length + " not equal to genome inputs length" + genome.getInputs());

            int i = 0;
            foreach (int inputNeuron in this.genome.getInputs())
                this.neuronInputValues.Add(inputNeuron, inputs[i++]);
        }

        public double[] calculateOutput()
        {
            Dictionary<int, double> cache = new Dictionary<int, double>();

            int i = 0;
            double[] output = new double[this.genome.getOutputs().Length];

            foreach (int outputNeuron in this.genome.getOutputs())
                output[i++] = this.getOutput(outputNeuron, cache);

            return output;
        }

        private double getOutput(int neuron, Dictionary<int, double> cache)
        {
            if (cache.ContainsKey(neuron))
                return cache[neuron];

            double sum = 0;

            foreach (Synapse synapse in this.genome.getSynapses())
            {
                if (synapse.to == neuron && synapse.enabled)
                {
                    if (this.genome.isInputNeuron(synapse.from))
                        sum += this.neuronInputValues[synapse.from] * synapse.weight;
                    else
                        sum += this.getOutput(synapse.from, cache) * synapse.weight;
                }
            }

            double d = activate(sum);
            cache.Add(neuron, d);
            return d;
        }

        private double activate(double x)
        {
            return 1.0f / (1.0f + (float)Math.Exp(-4.9 * x));
        }
    }
}
