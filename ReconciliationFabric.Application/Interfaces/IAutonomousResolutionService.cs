public interface IAutonomousResolutionService
{
    Task<ResolutionExecutionResult> ExecuteAsync(
        Discrepancy discrepancy);
}