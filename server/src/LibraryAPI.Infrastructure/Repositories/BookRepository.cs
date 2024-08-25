using Microsoft.EntityFrameworkCore;
using LibraryAPI.Domain.Entities;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(DbContext context) : base(context) { }

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
        return await _dbSet.AsNoTracking()
            .Include(b => b.Author)
            .Where(b => EF.Functions.Like(b.Author.FullName, $"%{authorFullName}%"))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByUserFullNameAsync(string userFullName)
    {
        return await _dbSet.AsNoTracking()
            .Include(b => b.User)
            .Where(b => EF.Functions.Like(b.User.FullName, $"%{userFullName}%"))
            .ToListAsync();
    }
}
