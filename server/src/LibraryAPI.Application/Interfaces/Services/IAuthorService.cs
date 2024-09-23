using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Interfaces
{
    public interface IAuthorService : IBaseService<Author>
    {
        Task<PaginatedResponseDto<AuthorResponseDto>> GetAuthorsPaginatedAsync(PaginatedRequestDto request);
        Task<IEnumerable<AuthorResponseDto>> GetAllAuthorsAsync();
        Task<AuthorResponseDto> GetAuthorByIdAsync(int id);
        Task AddAuthorAsync(AuthorRequestDto request);
        Task UpdateAuthorAsync(int id, AuthorRequestDto request);
        Task DeleteAuthorAsync(int id);
        Task<IEnumerable<BookAdminResponseDto>> GetBooksByAuthorIdAsync(int authorId);
    }
}