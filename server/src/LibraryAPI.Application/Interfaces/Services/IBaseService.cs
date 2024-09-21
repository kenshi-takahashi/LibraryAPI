using LibraryAPI.Application.Common;

namespace LibraryAPI.Application.Interfaces
{
    public interface IBaseService<T>
    {
        Task<PaginatedList<T>> GetPaginatedAsync(int pageIndex, int pageSize);
    }
}