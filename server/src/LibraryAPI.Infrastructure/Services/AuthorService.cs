using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;

public class AuthorService : BaseService<Author>, IAuthorService
{
    private readonly IMapper _mapper;

    public AuthorService(IBaseRepository<Author> repository, IMapper mapper) : base(repository)
    {
        _mapper = mapper;
    }

    public async Task<PaginatedResponseDto<AuthorResponseDto>> GetAuthorsPaginatedAsync(PaginatedRequestDto request)
    {
        var paginatedList = await GetPaginatedAsync(request.PageIndex, request.PageSize);
        var response = _mapper.Map<PaginatedResponseDto<AuthorResponseDto>>(paginatedList);

        return response;
    }

}
