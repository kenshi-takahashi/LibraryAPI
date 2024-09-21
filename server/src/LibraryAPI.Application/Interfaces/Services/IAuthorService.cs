using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Interfaces
{
    public interface IAuthorService : IBaseService<Author>
    {
        Task<PaginatedResponseDto<AuthorResponseDto>> GetAuthorsPaginatedAsync(PaginatedRequestDto request);
    }
}