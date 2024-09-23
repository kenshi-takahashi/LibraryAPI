using LibraryAPI.Application.DTOs;

public interface IBookService
{
    Task<IEnumerable<BookAdminResponseDto>> GetAllBooksAsync();
    Task<BookAdminResponseDto> GetBookByIdAsync(int id);
    Task<IEnumerable<BookAdminResponseDto>> GetBooksByISBNAsync(string isbn);
    Task AddBookAsync(BookCreateRequestDto bookDto);
    Task UpdateBookAsync(int id, BookUpdateRequestDto bookDto);
    Task DeleteBookAsync(int id);
    Task BorrowBookAsync(int bookId, int userId);
    Task<string> AddBookImageAsync(int bookId, string imagePath);
    Task NotifyUsersAboutReturnDatesAsync();
}
