using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Repositories
{
    public class AuthorRepositoryTests
    {
        private LibraryAPIDbContext _context;
        private AuthorRepository _authorRepository;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryAPIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryAPIDbContext(options);
            _authorRepository = new AuthorRepository(_context);
        }

        [Fact]
        public async Task GetBooksByAuthorIdAsync_ShouldReturnBooks()
        {
            // Arrange
            var author = new Author
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1985, 5, 15),
                Country = "USA"
            };
            _context.Authors.Add(author);

            var book = new Book
            {
                Title = "Book1",
                AuthorId = author.Id,
                ISBN = "1234567890",
                Genre = "Fiction",
                IsAvailable = true
            };
            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            // Act
            var books = await _authorRepository.GetBooksByAuthorIdAsync(author.Id);

            // Assert
            Assert.Single(books);
            Assert.Equal("Book1", books.First().Title);
        }
    }
}
