using AutoMapper;
using FluentValidation;
using Moq;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Application.Common;

namespace LibraryAPI.Tests.Services
{
    public class BaseServiceTests
    {
        private readonly AuthorService _authorService;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<AuthorRequestDto>> _validatorMock;

        public BaseServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<AuthorRequestDto>>();
            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAuthorsPaginatedAsync_ShouldReturnPaginatedAuthors()
        {
            // Arrange
            var paginatedList = new PaginatedList<Author>(
                new List<Author> { new Author { FirstName = "John", LastName = "Doe" } }, 1, 1);
            _authorRepositoryMock.Setup(r => r.GetPaginatedItemsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedList.Items);
            _authorRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<PaginatedResponseDto<AuthorResponseDto>>(It.IsAny<PaginatedList<Author>>()))
                .Returns(new PaginatedResponseDto<AuthorResponseDto> { Items = new List<AuthorResponseDto> { new AuthorResponseDto { FirstName = "John", LastName = "Doe" } } });

            // Act
            var result = await _authorService.GetAuthorsPaginatedAsync(new PaginatedRequestDto());

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("John", result.Items.First().FirstName);
        }
    }
}