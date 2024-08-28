public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
}
