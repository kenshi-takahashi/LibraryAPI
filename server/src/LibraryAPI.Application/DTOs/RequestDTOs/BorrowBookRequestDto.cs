namespace LibraryAPI.Application.DTOs
{
    public class BorrowBookRequestDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}
