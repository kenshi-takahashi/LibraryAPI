using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(LibraryAPIDbContext context) : base(context) { }

    public async Task<Role> GetByNameAsync(string roleName)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == roleName);
    }
}