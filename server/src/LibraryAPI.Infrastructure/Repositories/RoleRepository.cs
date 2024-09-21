using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(LibraryAPIDbContext context) : base(context) { }
}