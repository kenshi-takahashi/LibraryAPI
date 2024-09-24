using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Application.Common;

namespace LibraryAPI.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly AuthorService _authorService;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<AuthorRequestDto>> _validatorMock;

        public AuthorServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<AuthorRequestDto>>();
            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ShouldReturnAllAuthors()
        {
            // Arrange
            var authors = new List<Author> { new Author { FirstName = "John", LastName = "Doe" } };
            _authorRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(authors);
            _mapperMock.Setup(m => m.Map<IEnumerable<AuthorResponseDto>>(authors))
                .Returns(new List<AuthorResponseDto> { new AuthorResponseDto { FirstName = "John", LastName = "Doe" } });

            // Act
            var result = await _authorService.GetAllAuthorsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("John", result.First().FirstName);
        }

        [Fact]
        public async Task AddAuthorAsync_ShouldAddAuthor_WhenValidRequest()
        {
            // Arrange
            var request = new AuthorRequestDto { FirstName = "John", LastName = "Doe" };
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.Map<Author>(request)).Returns(new Author { FirstName = "John", LastName = "Doe" });

            // Act
            await _authorService.AddAuthorAsync(request);

            // Assert
            _authorRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task AddAuthorAsync_ShouldThrowValidationException_WhenInvalidRequest()
        {
            // Arrange
            var request = new AuthorRequestDto { FirstName = "John", LastName = "Doe" };
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("FirstName", "Required") }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authorService.AddAuthorAsync(request));
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = new Author { FirstName = "John", LastName = "Doe" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(new AuthorResponseDto { FirstName = "John", LastName = "Doe" });

            // Act
            var result = await _authorService.GetAuthorByIdAsync(1);

            // Assert
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldThrowKeyNotFoundException_WhenAuthorNotFound()
        {
            // Arrange
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Author)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _authorService.GetAuthorByIdAsync(1));
        }

        [Fact]
        public async Task UpdateAuthorAsync_ShouldUpdateAuthor_WhenAuthorExistsAndRequestIsValid()
        {
            // Arrange
            var author = new Author { FirstName = "John", LastName = "Doe" };
            var request = new AuthorRequestDto { FirstName = "Jane", LastName = "Smith" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _authorService.UpdateAuthorAsync(1, request);

            // Assert
            _mapperMock.Verify(m => m.Map(request, author), Times.Once);
            _authorRepositoryMock.Verify(r => r.UpdateAsync(author), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = new Author { FirstName = "John", LastName = "Doe" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);

            // Act
            await _authorService.DeleteAuthorAsync(1);

            // Assert
            _authorRepositoryMock.Verify(r => r.DeleteAsync(author), Times.Once);
        }
    }
}