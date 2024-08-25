using LibraryAPI.Domain.Entities;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetByISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetByTitleAsync(string title);
    Task<IEnumerable<Book>> GetByGenreAsync(string genre);
    Task<IEnumerable<Book>> GetByAvailableAsync(bool isAvailable);
    Task<IEnumerable<Book>> GetByAuthorFullNameAsync(string authorFullName);
    Task<IEnumerable<Book>> GetByUserFullNameAsync(string userFullName);
}