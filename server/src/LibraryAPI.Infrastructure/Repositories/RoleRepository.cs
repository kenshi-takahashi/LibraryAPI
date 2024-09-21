using LibraryAPI.Domain.Entities;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(LibraryAPIDbContext context) : base(context) { }
}