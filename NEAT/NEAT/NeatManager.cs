using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public class NeatManager
    {
        private List<Species> _species;
        private int _generation;

        private String _gameName;
        private game2048 _game;

        private int[][] _inputs;
        private game2048.Direction[] _outputs;

        public NeatManager(String game)
        {
            Species seedSpecies = new Species();
            seedSpecies.randomGenome();

            this._species = new List<Species>();
            this._species.Add(seedSpecies);

            this._generation = 1;

            this._gameName = game;
        }

        public void train()
        {
            if(_gameName == "2048")
                _game = new game2048();

            _inputs = new int[4][];
            {
                _inputs[0] = new int[] { 1, 1, 1, 1 };
                _inputs[1] = new int[] { 1, 1, 1, 1 };
                _inputs[2] = new int[] { 1, 1, 1, 1 };
                _inputs[3] = new int[] { 1, 1, 1, 1 };
            }

            _outputs = new game2048.Direction[4];
            {
                _outputs[0] = game2048.Direction.Left;
                _outputs[1] = game2048.Direction.Right;
                _outputs[2] = game2048.Direction.Up;
                _outputs[3] = game2048.Direction.Down;
            }



            for (int g = 0; g < 10; g++)
            {
                // Generate new generation
                // newGeneration();

                // Save generation
            }
        }

        public void newGeneration()
        {
            InfoManager.addLine("Generation " + this._generation);

            // 1. Calculate Deep Fitness
            // Calculate best genome for candidate
            InfoManager.addLine("Deep Fitness for " + _species.Count + " _species");
            for (int i = 0; i < this._species.Count; i++)
            {
                Fitness.deepFitness(this._species[i]);
            }

            // 2. Calculate adjusted fitness of all _species
            int totalFitness = 5000;

            // 3. Survival of fittest
            InfoManager.addLine("Fittests for " + _species.Count + " _species");
            for (int i = 0; i < this._species.Count; i++)
            {
                Fitness.fittest(this._species[i], totalFitness);
            }

            // 4. Extinction of useless _species
            InfoManager.addLine("Simulate extinction for " + _species.Count + " speices");
            extinction(_species);

            // 5. Simulate mating
            InfoManager.addLine("Simulate mating for " + _species.Count + " speices");
            for (int i = 0; i < this._species.Count; i++)
            {
                mate(_species[i]);
            }



            InfoManager.addLine("========================================================================");
            this._generation++;
        }

        public void extinction(List<Species> _species)
        {
            int keepSpeciesCount = _species.Count - (int)Math.Floor(_species.Count * Config.EXTINCTION_CANDIDATE);

            _species.OrderBy(o => o.averageFitness);
            List<Species> goingExtinct = new List<Species>();

            InfoManager.addLine("Keeping " + keepSpeciesCount + " _species (out of " + _species.Count + ")");
            for (int i = keepSpeciesCount; i < this._species.Count; i++)
            {
                _species[i].extinctionCounter++;

                if (_species[i].extinctionCounter >= Config.EXTINCTION_GENERATIONS)
                {
                    InfoManager.addLine("Species " + _species[i].instanceID + " went extinct");

                    goingExtinct.Add(_species[i]);
                }
            }

            for (int i = 0; i < goingExtinct.Count; i++)
            {
                _species.Remove(goingExtinct[i]);
            }
        }


        private void mate(Species _species)
        {
            //throw new NotImplementedException();
        }
    }
}
