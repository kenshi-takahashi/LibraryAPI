using LibraryAPI.Domain.Entities;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book> GetByISBNAsync(string isbn);
}