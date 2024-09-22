using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
    {
        _authorService = authorService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAllAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<AuthorResponseDto>> GetAuthorById(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        return Ok(author);
    }

    [HttpGet("paginated")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<ActionResult<PaginatedResponseDto<AuthorResponseDto>>> GetAuthorsPaginated([FromQuery] PaginatedRequestDto request)
    {
        var authors = await _authorService.GetAuthorsPaginatedAsync(request);
        return Ok(authors);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> AddAuthor([FromBody] AuthorRequestDto request)
    {
        await _authorService.AddAuthorAsync(request);
        return CreatedAtAction(nameof(GetAllAuthors), new { }, null);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorRequestDto request)
    {
        await _authorService.UpdateAuthorAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        await _authorService.DeleteAuthorAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/books")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<ActionResult<IEnumerable<BookAdminResponseDto>>> GetBooksByAuthorId(int id)
    {
        var books = await _authorService.GetBooksByAuthorIdAsync(id);
        return Ok(books);
    }
}
