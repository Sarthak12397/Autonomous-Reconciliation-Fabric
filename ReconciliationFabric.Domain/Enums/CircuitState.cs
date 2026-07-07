namespace Kca.Infrastructure.CircuitBreaker;

public enum CircuitState
{
    Healthy,
    Degraded,
    Tripped,
    Recovering,
    Graduated,
    TerminalFault
}