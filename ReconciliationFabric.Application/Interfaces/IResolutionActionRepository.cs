public interface IResolutionActionRepository
{
    Task AddAsync(ResolutionAction entity);

    Task<IEnumerable<ResolutionAction>> GetByDiscrepancyIdAsync(
        Guid discrepancyId);
}