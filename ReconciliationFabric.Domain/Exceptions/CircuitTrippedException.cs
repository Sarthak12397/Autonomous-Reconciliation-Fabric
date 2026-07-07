using Kca.Infrastructure.CircuitBreaker;

public sealed class CircuitTrippedException: Exception
{
        public Guid CircuitId { get; }
        public string CircuitName {get; }

        public CircuitState State{get; }


        public CircuitTrippedException(
            Guid circuitId,
            string circuitName,
            CircuitState state
        ) : base($"Circuit '{circuitName}' is {state}. Operation rejected.")
    {
        CircuitId = circuitId;
        CircuitName = circuitName;
        State = state;
    }
}