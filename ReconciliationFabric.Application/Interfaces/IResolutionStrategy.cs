public interface IResolutionStrategy
{
    ResolutionStrategyType Type { get; }

    bool CanResolve(Discrepancy discrepancy);

    Task<ResolutionExecutionResult> ExecuteAsync(
        Discrepancy discrepancy);
}