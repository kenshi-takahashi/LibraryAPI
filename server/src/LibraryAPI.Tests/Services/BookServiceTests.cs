using Moq;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using FluentValidation.Results;
using Xunit;

namespace LibraryAPI.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IValidator<BookCreateRequestDto>> _createValidatorMock;
        private readonly Mock<IValidator<BookUpdateRequestDto>> _updateValidatorMock;
        private readonly Mock<IValidator<BorrowBookRequestDto>> _borrowValidatorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheMock = new Mock<IMemoryCache>();
            _createValidatorMock = new Mock<IValidator<BookCreateRequestDto>>();
            _updateValidatorMock = new Mock<IValidator<BookUpdateRequestDto>>();
            _borrowValidatorMock = new Mock<IValidator<BorrowBookRequestDto>>();
            _userRepositoryMock = new Mock<IUserRepository>();

            // Set up the memory cache mock
            _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(new Mock<ICacheEntry>().Object);

            _bookService = new BookService(
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                _cacheMock.Object,
                _userRepositoryMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _borrowValidatorMock.Object
            );
        }

        [Fact]
        public async Task AddBookAsync_ValidBook_AddsBookSuccessfully()
        {
            // Arrange
            var bookDto = new BookCreateRequestDto { ISBN = "12345", Title = "Test Book", AuthorId = 1 };
            var book = new Book { Id = 1, ISBN = "12345", Title = "Test Book", AuthorId = 1 };

            _createValidatorMock.Setup(v => v.ValidateAsync(bookDto, default))
                .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(m => m.Map<Book>(bookDto)).Returns(book);

            // Act
            await _bookService.AddBookAsync(bookDto);

            // Assert
            _bookRepositoryMock.Verify(r => r.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task BorrowBookAsync_ValidBorrow_UpdatesBookAvailability()
        {
            // Arrange
            var borrowDto = new BorrowBookRequestDto { BookId = 1, UserId = 1 };
            var book = new Book { Id = 1, Title = "Test Book", IsAvailable = true };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
            _borrowValidatorMock.Setup(v => v.ValidateAsync(borrowDto, default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _bookService.BorrowBookAsync(borrowDto);

            // Assert
            Assert.False(book.IsAvailable);
            _bookRepositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            var expectedBooks = new List<BookAdminResponseDto> { new BookAdminResponseDto { Id = 1, Title = "Test Book" } };

            _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookAdminResponseDto>>(books)).Returns(expectedBooks);

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Book", result.First().Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_CachedBook_ReturnsCachedBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            var expectedResponse = new BookAdminResponseDto { Id = 1, Title = "Test Book" };

            object cachedBook = expectedResponse;
            _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedBook)).Returns(true);

            // Act
            var result = await _bookService.GetBookByIdAsync(1);

            // Assert
            Assert.Equal("Test Book", result.Title);
            _bookRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetBooksByISBNAsync_ReturnsBooksByISBN()
        {
            // Arrange
            var books = new List<Book> { new Book { ISBN = "12345", Title = "Test Book" } };
            var expectedBooks = new List<BookAdminResponseDto> { new BookAdminResponseDto { ISBN = "12345", Title = "Test Book" } };

            _bookRepositoryMock.Setup(r => r.GetByISBNAsync("12345")).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookAdminResponseDto>>(books)).Returns(expectedBooks);

            // Act
            var result = await _bookService.GetBooksByISBNAsync("12345");

            // Assert
            Assert.Single(result);
            Assert.Equal("12345", result.First().ISBN);
        }

        [Fact]
        public async Task UpdateBookAsync_ValidBook_UpdatesBook()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var bookDto = new BookUpdateRequestDto { ISBN = "12345", Title = "Updated Book", AuthorId = 1 };
            var book = new Book { Id = 1, ISBN = "12345", Title = "Test Book", AuthorId = 1 };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
            _updateValidatorMock.Setup(v => v.ValidateAsync(bookDto, default)).ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.Map(bookDto, book)).Callback(() => book.Title = "Updated Book");

            // Act
            var bookService = new BookService(
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                memoryCache,
                _userRepositoryMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _borrowValidatorMock.Object
            );

            await bookService.UpdateBookAsync(1, bookDto);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
            Assert.Equal("Updated Book", book.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_ValidId_DeletesBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

            // Act
            await _bookService.DeleteBookAsync(1);

            // Assert
            _bookRepositoryMock.Verify(r => r.DeleteAsync(book), Times.Once);
        }

        [Fact]
        public async Task AddBookImageAsync_ValidBook_AddsImage()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book", Image = null };
            string imagePath = "image.jpg";

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
            object cachedBook = null;
            _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedBook))
                      .Returns(false);

            // Act
            var result = await _bookService.AddBookImageAsync(1, imagePath);

            // Assert
            Assert.Equal(imagePath, result);
            _bookRepositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
        }
    }
}
