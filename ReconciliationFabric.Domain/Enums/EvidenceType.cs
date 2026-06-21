namespace Kca.Infrastructure.CircuitBreaker;

public enum CircuitSignalType
{
    OperationFailed,
    OperationSucceeded,
    ResourcePressureHigh,
    ResourcePressureNormal,
    LatencyExceeded,
    ConsecutiveFailure,
    CircuitBreakerSignal,
    RecoveryAttemptFailed,
    RecoveryAttemptSucceeded
}