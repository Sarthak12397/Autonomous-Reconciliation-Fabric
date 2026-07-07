using Kca.Infrastructure.CircuitBreaker;

public sealed class InvalidCircuitStateTransitionException : Exception
{
    public Guid CircuitId { get; }
    public string CircuitName { get; }
    public CircuitState From { get; }
    public CircuitState To { get; }

    public InvalidCircuitStateTransitionException(
        Guid circuitId,
        string circuitName,
        CircuitState from,
        CircuitState to)
        : base($"Circuit '{circuitName}' ({circuitId}) cannot transition from {from} to {to}.")
    {
        CircuitId = circuitId;
        CircuitName = circuitName;
        From = from;
        To = to;
    }
}