﻿using NEAT.NEAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Simulations
{
    public class XOR
    {
        public double[][] getInputs()
        {
            double[][] inputs = new double[4][];

            inputs[0] = new double[] { 1, 0, 0 };
            inputs[1] = new double[] { 1, 1, 1 };
            inputs[2] = new double[] { 1, 0, 1 };
            inputs[3] = new double[] { 1, 1, 0 };

            return inputs;
        }

        public double[][] getOutputs()
        {
            double[][] outputs = new double[4][];

            outputs[0] = new double[] { 0 };
            outputs[1] = new double[] { 0 };
            outputs[2] = new double[] { 1 };
            outputs[3] = new double[] { 1 };

            return outputs;
        }
        public double calculate(Genome genome)
        {
            double off = 0;
            for (int i = 0; i < 4; i++)
            {
                double[] inVal = getInputs()[i];

                double expectedOut = getOutputs()[i][0];
                double actualOut = genome.calculateMove(inVal)[0];

                off += Math.Abs(actualOut - expectedOut);
            }
            // subtract from 4 and square to increase fitness proportionally
            double fitness = 4 - off;

            if (fitness < 0)
                fitness = 0;

            return fitness * fitness;
        }
    }
}
