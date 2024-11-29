using Microsoft.EntityFrameworkCore;

namespace Askify.Shared.Pagination;

public static class QueryableExtensions
{
    public static async Task<PagedResponse<T>> PaginateAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalItems = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<T>(items, totalItems, pageNumber, pageSize);
    }
}