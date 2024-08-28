public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryAPIDbContext _context;

    public UnitOfWork(LibraryAPIDbContext context)
    {
        _context = context;
        Books = new BookRepository(_context);
        Authors = new AuthorRepository(_context);
        Users = new UserRepository(_context);
        Roles = new RoleRepository(_context);
    }

    public IBookRepository Books { get; private set; }
    public IAuthorRepository Authors { get; private set; }
    public IUserRepository Users { get; private set; }
    public IRoleRepository Roles { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
