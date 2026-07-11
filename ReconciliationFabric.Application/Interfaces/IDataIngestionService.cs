public interface IDataIngestionService
{
    IAsyncEnumerable<ExternalRecord> FetchExternalAsync(
        SourceSystem source,
        ReconciliationWindow window);

    IAsyncEnumerable<InternalRecord> FetchInternalAsync(
        SourceSystem source,
        ReconciliationWindow window);
}