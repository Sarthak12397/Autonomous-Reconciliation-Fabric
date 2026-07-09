public record PagedResult<T>(
    IReadOnlyList<T> Items,
    Guid? NextCursor,
    int? TotalCount
);