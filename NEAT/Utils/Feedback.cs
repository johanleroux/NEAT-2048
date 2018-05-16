using System.Collections.Generic;

namespace NEAT.Utils
{
    public static class Feedback
    {
        public static List<int> fitnessPerGeneration = new List<int>();
        
        public static void addFitnessPerGeneration(int fitness)
        {
            fitnessPerGeneration.Add(fitness);
        }

        public static void clearFitnessPerGeneration()
        {
            fitnessPerGeneration.Clear();
        }
    }
}
