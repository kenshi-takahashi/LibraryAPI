using LibraryAPI.Domain.Entities;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role> GetByNameAsync(string roleName);
}