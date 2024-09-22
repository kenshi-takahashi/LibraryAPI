using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;

public class AuthorService : BaseService<Author>, IAuthorService
{
    private readonly IMapper _mapper;
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository, IMapper mapper) : base(authorRepository)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponseDto<AuthorResponseDto>> GetAuthorsPaginatedAsync(PaginatedRequestDto request)
    {
        var paginatedList = await GetPaginatedAsync(request.PageIndex, request.PageSize);
        var response = _mapper.Map<PaginatedResponseDto<AuthorResponseDto>>(paginatedList);

        return response;
    }

    public async Task<IEnumerable<AuthorResponseDto>> GetAllAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorResponseDto>>(authors);
    }

    public async Task AddAuthorAsync(AuthorRequestDto request)
    {
        var author = _mapper.Map<Author>(request);
        await _authorRepository.AddAsync(author);
    }

    public async Task<AuthorResponseDto> GetAuthorByIdAsync(int id)
{
    var author = await _authorRepository.GetByIdAsync(id);
    if (author == null) throw new KeyNotFoundException($"Author with ID {id} not found.");
    
    return _mapper.Map<AuthorResponseDto>(author);
}

public async Task UpdateAuthorAsync(int id, AuthorRequestDto request)
{
    var author = await _authorRepository.GetByIdAsync(id);
    if (author == null) throw new KeyNotFoundException($"Author with ID {id} not found.");

    _mapper.Map(request, author);
    await _authorRepository.UpdateAsync(author);
}

public async Task DeleteAuthorAsync(int id)
{
    var author = await _authorRepository.GetByIdAsync(id);
    if (author == null) throw new KeyNotFoundException($"Author with ID {id} not found.");

    await _authorRepository.DeleteAsync(author);
}


    public async Task<IEnumerable<BookAdminResponseDto>> GetBooksByAuthorIdAsync(int authorId)
    {
        var books = await _authorRepository.GetBooksByAuthorIdAsync(authorId);
        return _mapper.Map<IEnumerable<BookAdminResponseDto>>(books);
    }
}