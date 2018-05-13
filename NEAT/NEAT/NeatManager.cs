using NEAT.NEAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NEAT.NEAT
{
    public class NeatManager
    {
        private String _gameName;
        public game2048 _game;

        private int innovationNumber = 1;
        private int populationSize = 1000;

        public int inputSize, outputSize;

        public readonly PopulationManager popManager;

        public NeatManager(String game)
        {
            popManager = new PopulationManager(this);

            this._gameName = game;

            if (_gameName == "2048")
                _game = new game2048();

            inputSize = 16;
            outputSize = 1;

            trainGenerations(10);
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
            
            // Run for g generations
            for(int g = 1; g <= generations; g++)
            {
                this.popManager.newGeneration();

                Genome best = this.popManager.latestFitness;

                if (best.getFitness() > highestFitness)
                {
                    bestGenome = best;
                    bestGeneration = g;
                    highestFitness = bestGenome.getFitness();
                }
            }

            InfoManager.addLine("Best generation " + bestGeneration + " with a fitness score of " + highestFitness + " from genome " + bestGenome.getID());
            InfoManager.addLine("The system had " + bestGenome.getHiddenNodes().Count + " hidden nodes and " + bestGenome.getActiveConnections().Count() + " enabled connections");

            // Run generation
        }
        
        public double runGenome(Genome genome)
        {
            _game = new game2048();
            _game.initGame();
            int sameMoves = 0;
            int prevScore = 0;
            game2048.Direction prevMove = game2048.Direction.Down;

            while(_game.state == game2048.GameState.Playing)
            {
                int[] inputs = new int[16];

                inputs = _game.getInputs();

                double[] ans = genome.calculateMove(inputs);

                game2048.Direction move;
                if(ans[0] < 0.25)
                    move = game2048.Direction.Down;
                else if (ans[0] < 0.5)
                    move = game2048.Direction.Up;
                else if (ans[0] < 0.75)
                    move = game2048.Direction.Left;
                else
                    move = game2048.Direction.Right;

                if (prevMove == move && prevScore == _game.score)
                    sameMoves++;

                if (sameMoves > 5)
                {
                    Console.WriteLine("SAME MOVE 5 TIMES WITH NO ACTION: " + _game.score);
                    _game.state = game2048.GameState.GameOver;
                }

                _game.moveTiles(move);
                prevMove = move;
                prevScore = _game.score;
            }

            return _game.score;
        }
    }
}
