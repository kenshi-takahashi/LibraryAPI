using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(DbContext context) : base(context) { }
}