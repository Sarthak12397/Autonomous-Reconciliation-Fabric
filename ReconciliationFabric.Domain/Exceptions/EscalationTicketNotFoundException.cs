public sealed class EscalationTicketNotFoundException: Exception
{
    public Guid TicketId{get;}

    public EscalationTicketNotFoundException
    (

        Guid ticketId
    ):base($"Escalation ticket '{ticketId}' was not found.")
    {
        TicketId = ticketId;
        
    }
}
