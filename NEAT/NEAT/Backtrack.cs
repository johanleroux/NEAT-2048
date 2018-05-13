using NEAT.NEAT.Models;
using System;
using System.Collections.Generic;

namespace NEAT.NEAT
{
    class Backtrack
    {
        private Genome genome;
        private Dictionary<int, double> nodeInputValues = new Dictionary<int, double>();

        public Backtrack(Genome genome, double[] inputs)
        {
            this.genome = genome;

            if (genome.getInputs().Length != inputs.Length)
                throw new Exception("Input size " + inputs.Length + " not equal to genome inputs length" + genome.getInputs());

            int i = 0;
            foreach (int inputNode in this.genome.getInputs())
                this.nodeInputValues.Add(inputNode, inputs[i++]);
        }

        public double[] calculateOutput()
        {
            Dictionary<int, double> cache = new Dictionary<int, double>();

            int i = 0;
            double[] output = new double[this.genome.getOutputs().Length];

            foreach (int outputNode in this.genome.getOutputs())
                output[i++] = this.getOutput(outputNode, cache);

            return output;
        }

        private double getOutput(int node, Dictionary<int, double> cache)
        {
            if (cache.ContainsKey(node))
                return cache[node];

            double sum = 0;

            foreach (Synapse synapse in this.genome.getSynapses())
            {
                if (synapse.to == node && synapse.enabled)
                {
                    if (this.genome.isInputNode(synapse.from))
                        sum += this.nodeInputValues[synapse.from] * synapse.weight;
                    else
                        sum += this.getOutput(synapse.from, cache) * synapse.weight;
                }
            }

            double d = activate(sum);
            cache.Add(node, d);
            return d;
        }

        private double activate(double x)
        {
            return 1.0f / (1.0f + (float)Math.Exp(-4.9 * x));
        }
    }
}
