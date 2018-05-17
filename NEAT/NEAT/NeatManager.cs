using NEAT.NEAT.Models;
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
        public game2048 _game;

        private int innovationNumber = 1;
        private int populationSize = 500;

        public int inputSize, outputSize;

        public readonly PopulationManager popManager;

        public int[] moves = new int[4];

        public NeatManager(String game, int nGenerations)
        {
            popManager = new PopulationManager(this);

            this._gameName = game;

            if (_gameName == "2048")
                _game = new game2048();

            inputSize = 16;
            outputSize = 4;

            for (int i = 0; i < 4; i++)
                moves[i] = 0;

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

            for (int i = 0; i < 4; i++)
                Console.WriteLine(moves[i]);

            // Run generation
            demoGenome(bestGenome);
        }

        public double runGenome(Genome genome)
        {
            _game = new game2048();
            _game.initGame();

            int prevScore = 0;

            while(_game.state == game2048.GameState.Playing)
            {
                double[] inputs = new double[16];

                inputs = _game.getInputs();

                double[] ans = genome.calculateMove(inputs);
                int makeMove = bestMove(ans);

                game2048.Direction move;
                if (makeMove == 0)
                {
                    move = game2048.Direction.Down;
                    //moves[0]++;
                }
                else if (makeMove == 1)
                {
                    move = game2048.Direction.Up;
                    //moves[1]++;
                }
                else if (makeMove == 2)
                {
                    move = game2048.Direction.Left;
                    //moves[2]++;
                }
                else
                {
                    move = game2048.Direction.Right;
                    //moves[3]++;
                }

                _game.moveTiles(move);

                prevScore = _game.score;
            }

            return _game.score;
        }

        private int bestMove(double[] ans)
        {
            int highest = 0;
            for(int i = 1; i < 4; i++)
            {
                if (ans[i] > ans[highest])
                    highest = i;
            }

            moves[highest]++;

            return highest;
        }

        private void demoGenome(Genome genome)
        {
            _game = new game2048();
            _game.background = false;
            _game.initGame();
            _game.background = false;
            _game.load();
            _game.StartPosition = FormStartPosition.CenterScreen;
            _game.Show();

            int sameMoves = 0;
            int prevScore = 0;
            game2048.Direction prevMove = game2048.Direction.Down;

            while (_game.state == game2048.GameState.Playing)
            {
                double[] inputs = new double[16];

                inputs = _game.getInputs();

                double[] ans = genome.calculateMove(inputs);

                game2048.Direction move;
                if (ans[0] < 0.25)
                    move = game2048.Direction.Down;
                else if (ans[0] < 0.5)
                    move = game2048.Direction.Up;
                else if (ans[0] < 0.75)
                    move = game2048.Direction.Left;
                else
                    move = game2048.Direction.Right;

                if (prevMove == move && prevScore == _game.score)
                    sameMoves++;
                else
                    sameMoves = 0;

                if (sameMoves > 5)
                    _game.state = game2048.GameState.GameOver;

                _game.moveTiles(move);
                prevMove = move;
                prevScore = _game.score;

                Console.WriteLine(move);

                _game.Refresh();

                Thread.Sleep(1000/2);
            }
        }
    }
}
