using LibraryAPI.Application.Common;
using LibraryAPI.Application.Interfaces;

public class BaseService<T> : IBaseService<T> where T : class
{
    private readonly IBaseRepository<T> _repository;

    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<T>> GetPaginatedAsync(int pageIndex, int pageSize)
    {
        var items = await _repository.GetPaginatedItemsAsync(pageIndex, pageSize);
        var totalCount = await _repository.GetTotalCountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedList<T>(items.ToList(), pageIndex, totalPages);
    }
}
