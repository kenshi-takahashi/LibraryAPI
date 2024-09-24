using Microsoft.EntityFrameworkCore;
using LibraryAPI.Domain.Entities;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LibraryAPIDbContext context) : base(context) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        return await _dbSet.AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.Role.Name.Contains(role))
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(string firstName, string lastName)
    {
        return await _dbSet.AsNoTracking()
            .Where(u => u.FirstName.Contains(firstName) && u.LastName.Contains(lastName))
            .ToListAsync();
    }

}
