using Microsoft.EntityFrameworkCore;
using LibraryAPI.Domain.Entities;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(LibraryAPIDbContext context) : base(context) { }

    public async Task<IEnumerable<Book>> GetByISBNAsync(string isbn)
    {
        return await _dbSet.AsNoTracking()
            .Where(b => b.ISBN.Contains(isbn))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByTitleAsync(string title)
    {
        return await _dbSet.AsNoTracking()
            .Where(b => b.Title.Contains(title))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByGenreAsync(string genre)
    {
        return await _dbSet.AsNoTracking()
            .Where(b => b.Genre.Contains(genre))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAvailableAsync(bool isAvailable)
    {
        return await _dbSet.AsNoTracking()
            .Where(b => b.IsAvailable == isAvailable)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorFullNameAsync(string authorFullName)
    {
        var names = authorFullName.Split(' ', 2);
        var firstName = names[0];
        var lastName = names.Length > 1 ? names[1] : string.Empty;

        return await _dbSet.AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.Author.FirstName == firstName &&
                        (string.IsNullOrEmpty(lastName) || b.Author.LastName == lastName))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByUserFullNameAsync(string userFullName)
    {
        var names = userFullName.Split(' ', 2);
        var firstName = names[0];
        var lastName = names.Length > 1 ? names[1] : string.Empty;

        return await _dbSet.AsNoTracking()
            .Include(b => b.User)
            .Where(b => b.User.FirstName == firstName &&
                        (string.IsNullOrEmpty(lastName) || b.User.LastName == lastName))
            .ToListAsync();
    }
}