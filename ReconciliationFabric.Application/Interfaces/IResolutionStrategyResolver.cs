public interface IResolutionStrategyResolver
{
    IResolutionStrategy Resolve(Discrepancy discrepancy);

    bool IsSafeToAutoResolve(Discrepancy discrepancy);
}