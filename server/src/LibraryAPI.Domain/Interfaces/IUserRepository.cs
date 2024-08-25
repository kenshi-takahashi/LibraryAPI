using LibraryAPI.Domain.Entities;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByRoleAsync(string role);
    Task<User> GetByFullNameAsync(string fullName);
}