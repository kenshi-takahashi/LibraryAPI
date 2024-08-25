using LibraryAPI.Domain.Entities;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleAsync(string role);
    Task<IEnumerable<User>> GetByFullNameAsync(string fullName);
}