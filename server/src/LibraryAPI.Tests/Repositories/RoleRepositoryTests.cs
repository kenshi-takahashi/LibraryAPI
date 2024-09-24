using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Repositories
{
    public class RoleRepositoryTests
    {
        private readonly LibraryAPIDbContext _context;
        private readonly RoleRepository _roleRepository;

        public RoleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryAPIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryAPIDbContext(options);
            _roleRepository = new RoleRepository(_context);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnRole_WhenRoleExists()
        {
            // Arrange
            var roleName = "Admin";
            var role = new Role { Id = 1, Name = roleName };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _roleRepository.GetByNameAsync(roleName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roleName, result.Name);
        }
    }
}
