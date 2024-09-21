using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IBaseService<Author> _authorService;

    public AuthorsController(IBaseService<Author> authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("paginated")]
    public async Task<ActionResult<PaginatedResponseDto<AuthorResponseDto>>> GetPaginatedAuthors([FromQuery] PaginatedRequestDto request)
    {
        var authors = await _authorService.GetPaginatedAsync(request.PageIndex, request.PageSize);
        return Ok(authors);
    }
}
