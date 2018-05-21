namespace NEAT.NEAT
{
    public static class Config
    {
        public const double MUTATION_CHANCE_NEW_NEURON = 0.01f;
        public const double MUTATION_CHANCE_NEW_SYNAPSE = 0.3f;
        public const double MUTATION_CHANCE_MUTATE_SYNAPSE = 0.7f;

        public const double SYNAPSE_WEIGHT_RANGE = 3.0f;
        public const double SYNAPSE_WEIGHT_RANDOM_CHANCE = 0.10f;
        public const double SYNAPSE_WEIGHT_PERTRUDE = 0.1f;

        public const double SPECIES_COMPATIBLE_DISTANCE = 1.25f;

        public const double DISTANCE_EXCESS_WEIGHT   = 1.0f;
        public const double DISTANCE_DISJOINT_WEIGHT = 1.0f;
        public const double DISTANCE_WEIGHTS_WEIGHT  = 0.4f;

        public const double GENERATION_ELIMINATION_PERCENTAGE = 0.85f;

        public const double BREED_CROSS_CHANCE = 0.75f;

        public const double NEURON_DISABLE_CHANCE = 0.75f;
    }
}
