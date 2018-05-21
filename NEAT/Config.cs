namespace NEAT.NEAT
{
    public static class Config
    {
        public const int BABIES_PER_GENERATION = 20;

        public const double FITTEST_PERCENTAGE = 20;

        public const int    EXTINCTION_GENERATIONS = 3;
        public const double EXTINCTION_CANDIDATE = 0.4f;

        public const double MUTATION_NEW_NODE_CHANCE = 0.003f;
        public const double MUTATION_NEW_CONNECTION_CHANCE = 0.3f;

        public const double MUTATION_WEIGHT_CHANCE = 0.7f;
        public const double MUTATION_WEIGHT_CHANCE_RANDOM_RANGE = 3.0f;
        public const double MUTATION_WEIGHT_RANDOM_CHANCE = 0.10f;
        public const double MUTATION_WEIGHT_MAX_DISTURBANCE = 0.1f;

        public const double SPECIES_COMPATIBILTY_DISTANCE = 1.25f;

        public const double DISTANCE_EXCESS_WEIGHT   = 1.0f;
        public const double DISTANCE_DISJOINT_WEIGHT = 1.0f;
        public const double DISTANCE_WEIGHTS_WEIGHT  = 0.4f;

        public const double GENERATION_ELIMINATION_PERCENTAGE = 0.85f;

        public const double BREED_CROSS_CHANCE = 0.75f;

        public const double GENE_DISABLE_CHANCE = 0.75f;
    }
}
