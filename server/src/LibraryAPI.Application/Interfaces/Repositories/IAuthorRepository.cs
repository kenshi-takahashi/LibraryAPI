using LibraryAPI.Domain.Entities;

public interface IAuthorRepository : IBaseRepository<Author>
{
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
}