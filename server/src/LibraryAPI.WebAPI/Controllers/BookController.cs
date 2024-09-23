using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return book != null ? Ok(book) : NotFound();
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetBooksByISBN(string isbn)
        {
            var books = await _bookService.GetBooksByISBNAsync(isbn);
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookCreateRequestDto bookDto)
        {
            await _bookService.AddBookAsync(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = bookDto.ISBN }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateRequestDto bookDto)
        {
            await _bookService.UpdateBookAsync(id, bookDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(int id, [FromQuery] int userId)
        {
            await _bookService.BorrowBookAsync(id, userId);
            return NoContent();
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> AddBookImage(int id, [FromForm] string imagePath)
        {
            var result = await _bookService.AddBookImageAsync(id, imagePath);
            return Ok(result);
        }

        [HttpPost("check-return-dates")]
        public async Task<IActionResult> CheckReturnDates()
        {
            await _bookService.NotifyUsersAboutReturnDatesAsync();
            return Ok("Уведомления отправлены");
        }
    }
}
