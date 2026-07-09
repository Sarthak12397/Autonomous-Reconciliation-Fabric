public interface IEscalationTicketRepository
{
    Task<EscalationTicket?> GetByIdAsync(Guid id);

    Task<EscalationTicket?> GetByDiscrepancyIdAsync(Guid discrepancyId);

    Task<IEnumerable<EscalationTicket>> GetOpenTicketsAsync();

    Task<IEnumerable<EscalationTicket>> GetBreachedTicketsAsync();

    Task<int> CountOpenByJobAsync(Guid jobId);

    Task AddAsync(EscalationTicket entity);

    Task UpdateAsync(EscalationTicket entity);
}