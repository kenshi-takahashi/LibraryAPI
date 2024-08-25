using Microsoft.EntityFrameworkCore;
using LibraryAPI.Domain.Entities;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<User>> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking()
            .Where(u => u.Email.Contains(email))
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        return await _dbSet.AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.Role.Name.Contains(role))
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(string fullName)
    {
        return await _dbSet.AsNoTracking()
            .Where(u => u.FullName.Contains(fullName))
            .ToListAsync();
    }
}
