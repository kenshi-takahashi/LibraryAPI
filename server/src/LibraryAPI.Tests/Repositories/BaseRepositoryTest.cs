using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Repositories
{
    public class BaseRepositoryTests
    {
        private LibraryAPIDbContext _context;
        private AuthorRepository _authorRepository;

        public BaseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryAPIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryAPIDbContext(options);
            _authorRepository = new AuthorRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAuthor()
        {
            // Arrange
            var author = new Author
            {
                FirstName = "AuthorFirst",
                LastName = "AuthorLast",
                DateOfBirth = new DateTime(1990, 1, 1),
                Country = "CountryName"
            };

            // Act
            await _authorRepository.AddAsync(author);
            var authors = await _authorRepository.GetAllAsync();

            // Assert
            Assert.Single(authors);
            Assert.Equal("AuthorFirst", authors.First().FirstName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAuthors()
        {
            // Arrange
            _context.Authors.AddRange(
                new Author
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Country = "USA"
                },
                new Author
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1995, 1, 1),
                    Country = "UK"
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var authors = await _authorRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, authors.Count());
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor()
        {
            // Arrange
            var author = new Author
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Country = "USA"
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.GetByIdAsync(author.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAuthor()
        {
            // Arrange
            var author = new Author
            {
                FirstName = "OldName",
                LastName = "OldLastName",
                DateOfBirth = new DateTime(1980, 1, 1),
                Country = "OldCountry"
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Act
            author.FirstName = "NewName";
            await _authorRepository.UpdateAsync(author);
            var updatedAuthor = await _authorRepository.GetByIdAsync(author.Id);

            // Assert
            Assert.Equal("NewName", updatedAuthor.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAuthor()
        {
            // Arrange
            var author = new Author
            {
                FirstName = "AuthorToDelete",
                LastName = "LastName",
                DateOfBirth = new DateTime(1990, 1, 1),
                Country = "Country"
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Act
            await _authorRepository.DeleteAsync(author);
            var authors = await _authorRepository.GetAllAsync();

            // Assert
            Assert.Empty(authors);
        }

        [Fact]
        public async Task GetPaginatedItemsAsync_ShouldReturnPaginatedAuthors()
        {
            // Arrange
            _context.Authors.AddRange(
                new Author
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Country = "USA"
                },
                new Author
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1995, 1, 1),
                    Country = "UK"
                },
                new Author
                {
                    FirstName = "Mike",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1988, 1, 1),
                    Country = "Canada"
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var authorsPage1 = await _authorRepository.GetPaginatedItemsAsync(1, 2);
            var authorsPage2 = await _authorRepository.GetPaginatedItemsAsync(2, 2);

            // Assert
            Assert.Equal(2, authorsPage1.Count());
            Assert.Equal(1, authorsPage2.Count());
            Assert.Equal("John", authorsPage1.First().FirstName);
            Assert.Equal("Mike", authorsPage2.First().FirstName);
        }

        [Fact]
        public async Task GetTotalCountAsync_ShouldReturnTotalCountOfAuthors()
        {
            // Arrange
            _context.Authors.AddRange(
                new Author
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Country = "USA"
                },
                new Author
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1995, 1, 1),
                    Country = "UK"
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var totalCount = await _authorRepository.GetTotalCountAsync();

            // Assert
            Assert.Equal(2, totalCount);
        }
    }
}
