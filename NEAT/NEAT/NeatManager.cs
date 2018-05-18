using NEAT.NEAT.Models;
using NEAT.Simulations;
using NEAT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NEAT.NEAT
{
    public class NeatManager
    {
        private String _gameName;
        public game2048 _2048;
        public XOR _xor;

        private int innovationNumber = 1;
        private int populationSize = 500;

        public int inputSize, outputSize;

        public readonly PopulationManager popManager;

        public int[] moves;

        public NeatManager(String game, int nGenerations)
        {
            popManager = new PopulationManager(this);

            this._gameName = game;

            if (_gameName == "2048")
            {
                inputSize = 16;
                outputSize = 1;

                moves = new int[1];
                for (int i = 0; i < 1; i++)
                    moves[i] = 0;
            }

            if (_gameName == "XOR")
            {
                inputSize = 3;
                outputSize = 1;

                moves = new int[1];
                for (int i = 0; i < 1; i++)
                    moves[i] = 0;
            }

            trainGenerations(nGenerations);
        }
        public int getNextInnovationNumber()
        {
            return this.innovationNumber++;
        }

        public void trainGenerations(int generations)
        {
            Genome bestGenome = null;
            int bestGeneration = -1;
            double highestFitness = -1;

            // Initialize population
            InfoManager.addLine("Initializing population");
            this.popManager.initialize(populationSize);

            // Reset Feedback
            Feedback.clearFitnessPerGeneration();
            
            // Run for g generations
            for(int g = 1; g <= generations; g++)
            {
                this.popManager.newGeneration();

                Genome best = this.popManager.latestFitness;

                if (best == null || best.getFitness() > highestFitness)
                {
                    bestGenome = best;
                    bestGeneration = g;
                    highestFitness = bestGenome.getFitness();
                }

                Feedback.addFitnessPerGeneration((int) Math.Round(best.getFitness()));

                InfoManager.addLine("Generation " + g + " built with a fitness score of " + best.getFitness());
                InfoManager.clearLine();
            }

            InfoManager.addLine("Best generation " + bestGeneration + " with a fitness score of " + highestFitness + " from genome " + bestGenome.getID());
            InfoManager.addLine("The system had " + bestGenome.getHiddenNodes().Count + " hidden nodes and " + bestGenome.getActiveConnections().Count() + " enabled connections");

            if(_gameName == "2048")
                demo2048(bestGenome);
        }

        public double runGenome(Genome genome)
        {
            if (_gameName == "2048")
                return run2048(genome);
            if (_gameName == "XOR")
            {
                _xor = new XOR();
                return _xor.calculate(genome);
            }

            return 0f;
        }

        private double run2048(Genome genome)
        {
            _2048 = new game2048();
            _2048.initGame();

            while (_2048.state == game2048.GameState.Playing)
            {
                double[] inputs = new double[16];
                inputs = _2048.getInputs();

                double[] ans = genome.calculateMove(inputs);
                game2048.Direction move;

                if (ans[0] < 0.25)
                    move = game2048.Direction.Down;
                else if (ans[0] < 0.50)
                    move = game2048.Direction.Up;
                else if (ans[0] < 0.75)
                    move = game2048.Direction.Left;
                else
                    move = game2048.Direction.Right;

                _2048.moveTiles(move);
            }

            return _2048.score;
        }

        private void demo2048(Genome genome)
        {
            InfoManager.clearLine();

            _2048 = new game2048();
            _2048.background = false;
            _2048.initGame();
            _2048.background = false;
            _2048.StartPosition = FormStartPosition.CenterScreen;
            _2048.Show();

            String moves = "";

            while (_2048.state == game2048.GameState.Playing)
            {
                double[] inputs = new double[16];
                inputs = _2048.getInputs();

                double[] ans = genome.calculateMove(inputs);
                game2048.Direction move;

                if (ans[0] < 0.25)
                    move = game2048.Direction.Down;
                else if (ans[0] < 0.50)
                    move = game2048.Direction.Up;
                else if (ans[0] < 0.75)
                    move = game2048.Direction.Left;
                else
                    move = game2048.Direction.Right;

                _2048.moveTiles(move);
                moves += ((string)move.ToString())[0];

                Console.WriteLine(move);

                _2048.Refresh();
                Thread.Sleep(1000/2);
            }

            InfoManager.addLine("Genome ran with a fitness score of " + _2048.score);
            InfoManager.addLine("Moves made: " + moves);
        }
    }
}
