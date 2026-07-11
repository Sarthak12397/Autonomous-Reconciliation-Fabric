using System;

public sealed class ResolutionExecutionResult
{
    public ResolutionActionResult Result { get; }
    public string Notes { get; }
    public string Metadata { get; }

    public bool IsSuccess => Result == ResolutionActionResult.Success;

    private ResolutionExecutionResult(
        ResolutionActionResult result,
        string notes,
        string metadata)
    {
        Result = result;
        Notes = notes ?? string.Empty;
        Metadata = metadata ?? "{}";
    }

    public static ResolutionExecutionResult Success(
        string notes,
        string metadata)
    {
        return new ResolutionExecutionResult(
            ResolutionActionResult.Success,
            notes,
            metadata);
    }

    public static ResolutionExecutionResult Failed(
        string notes,
        string metadata)
    {
        return new ResolutionExecutionResult(
            ResolutionActionResult.Failed,
            notes,
            metadata);
    }

    public static ResolutionExecutionResult Skipped(
        string notes)
    {
        return new ResolutionExecutionResult(
            ResolutionActionResult.Skipped,
            notes,
            "{}");
    }
}