using LibraryAPI.Domain.Entities;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryAPIDbContext context) : base(context) { }
}