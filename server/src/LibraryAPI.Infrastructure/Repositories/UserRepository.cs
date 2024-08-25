using Microsoft.EntityFrameworkCore;
using LibraryAPI.Domain.Entities;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Contains(email));
    }

    public async Task<User> GetByRoleAsync(string role)
    {
        return await _dbSet.AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Role.Name.Contains(role));
    }

    public async Task<User> GetByFullNameAsync(string fullName)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.FullName.Contains(fullName));
    }
}
