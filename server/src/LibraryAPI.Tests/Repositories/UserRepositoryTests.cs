using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly LibraryAPIDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryAPIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryAPIDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = 1,
                Email = email,
                FirstName = "John",
                LastName = "Doe",
                Role = new Role { Id = 1, Name = "Admin" },
                Password = "TestPassword123"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetByRoleAsync_ShouldReturnUsers_WhenRoleExists()
        {
            // Arrange
            var roleName = "Admin";
            var role = new Role { Id = 1, Name = roleName };
            var users = new List<User>
            {
                new User { Id = 1, Email = "admin1@example.com", FirstName = "Admin1", LastName = "User", Role = role, Password = "Password123" },
                new User { Id = 2, Email = "admin2@example.com", FirstName = "Admin2", LastName = "User", Role = role, Password = "Password123" }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByRoleAsync(roleName);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, u => Assert.Equal(roleName, u.Role.Name));
        }


        [Fact]
        public async Task GetByFullNameAsync_ShouldReturnUsers_WhenFullNameMatches()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var user = new User { Id = 1, FirstName = firstName, LastName = lastName, Email = "johndoe@example.com", Password = "TestPassword123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByFullNameAsync(firstName, lastName);

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains(result, u => u.FirstName == firstName && u.LastName == lastName);
        }


    }
}
