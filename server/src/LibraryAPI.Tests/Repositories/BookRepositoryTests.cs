using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Repositories
{
    public class BookRepositoryTests
    {
        private LibraryAPIDbContext _context;
        private IBookRepository _bookRepository;

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryAPIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryAPIDbContext(options);
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task GetByISBNAsync_ShouldReturnBooksWithMatchingISBN()
        {
            // Arrange
            _context.Books.AddRange(
                new Book { ISBN = "123-456-789", Title = "Test Book 1", Genre = "Fiction", IsAvailable = true },
                new Book { ISBN = "987-654-321", Title = "Test Book 2", Genre = "Non-Fiction", IsAvailable = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByISBNAsync("123");

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Book 1", result.First().Title);
        }

        [Fact]
        public async Task GetByTitleAsync_ShouldReturnBooksWithMatchingTitle()
        {
            // Arrange
            _context.Books.AddRange(
                new Book { ISBN = "123-456-789", Title = "Learn C#", Genre = "Programming", IsAvailable = true },
                new Book { ISBN = "987-654-321", Title = "C# Basics", Genre = "Programming", IsAvailable = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByTitleAsync("C#");

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByGenreAsync_ShouldReturnBooksWithMatchingGenre()
        {
            // Arrange
            _context.Books.AddRange(
                new Book { ISBN = "123-456-789", Title = "Fiction Book", Genre = "Fiction", IsAvailable = true },
                new Book { ISBN = "987-654-321", Title = "History Book", Genre = "History", IsAvailable = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByGenreAsync("Fiction");

            // Assert
            Assert.Single(result);
            Assert.Equal("Fiction Book", result.First().Title);
        }

        [Fact]
        public async Task GetByAvailableAsync_ShouldReturnOnlyAvailableBooks()
        {
            // Arrange
            _context.Books.AddRange(
                new Book { ISBN = "123-456-789", Title = "Available Book", Genre = "Fiction", IsAvailable = true },
                new Book { ISBN = "987-654-321", Title = "Not Available Book", Genre = "Fiction", IsAvailable = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByAvailableAsync(true);

            // Assert
            Assert.Single(result);
            Assert.Equal("Available Book", result.First().Title);
        }

        [Fact]
        public async Task GetByAuthorFullNameAsync_ShouldReturnBooksByAuthorFullName()
        {
            // Arrange
            var author = new Author { FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now, Country = "USA" }; // Added Country
            _context.Authors.Add(author);
            _context.Books.Add(new Book { ISBN = "123-456-789", Title = "Book by John", Genre = "Fiction", IsAvailable = true, Author = author });
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByAuthorFullNameAsync("John Doe");

            // Assert
            Assert.Single(result);
            Assert.Equal("Book by John", result.First().Title);
        }


        [Fact]
        public async Task GetByUserFullNameAsync_ShouldReturnBooksByUserFullName()
        {
            // Arrange
            var user = new User
            {
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                Password = "Password123", // Set a password
                RoleId = 1 // Set RoleId if required
            };
            var author = new Author
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now,
                Country = "USA" // Added Country
            };

            _context.Users.Add(user);
            _context.Authors.Add(author);
            _context.Books.Add(new Book
            {
                ISBN = "123-456-789",
                Title = "Book by Jane",
                Genre = "Fiction",
                IsAvailable = true,
                Author = author,
                User = user
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetByUserFullNameAsync("Jane Smith");

            // Assert
            Assert.Single(result);
            Assert.Equal("Book by Jane", result.First().Title);
        }
    }
}