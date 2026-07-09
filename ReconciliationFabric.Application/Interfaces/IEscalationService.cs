public interface IEscalationService
{
    Task<EscalationTicket> CreateAsync(
        Discrepancy discrepancy,
        string reason);
}