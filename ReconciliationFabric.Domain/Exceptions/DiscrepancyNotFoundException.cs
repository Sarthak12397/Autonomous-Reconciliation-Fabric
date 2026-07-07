public sealed class DiscrepancyNotFoundException: Exception
{
    public Guid DiscrepancyId {get;}

    public DiscrepancyNotFoundException(Guid discrepancyId):base($"Discrepancy '{discrepancyId}' was not found.")
{

     DiscrepancyId = discrepancyId;
}
}