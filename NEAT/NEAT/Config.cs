using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.NEAT
{
    public static class Config
    {
        public const int BABIES_PER_GENERATION = 20;

        public const double FITTEST_PERCENTAGE = 20;

        public const int    EXTINCTION_GENERATIONS = 3;
        public const double EXTINCTION_CANDIDATE = 0.4;
    }
}
