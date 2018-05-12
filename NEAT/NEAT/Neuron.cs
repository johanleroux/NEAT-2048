using System;

namespace NEAT.NEAT
{
    public class Neuron
    {
        public Guid instanceID { get; private set; }

        public long innovationID, mutationID;

        public bool enabled;

        public NeuronType type;

        public Neuron()
        {
            this.instanceID = Guid.NewGuid();
            this.type = NeuronType.hidden;
            this.innovationID = 0;
            this.mutationID = 0;
            this.enabled = true;
        }

        public Neuron(NeuronType type, long innovationID, long mutationID)
        {
            this.instanceID = Guid.NewGuid();
            this.type = type;
            this.innovationID = innovationID;
            this.mutationID = mutationID;
        } 
    }
}
