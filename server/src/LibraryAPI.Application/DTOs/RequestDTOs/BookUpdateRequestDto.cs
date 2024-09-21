namespace LibraryAPI.Application.DTOs
{
    public class BookUpdateRequestDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public string? Image { get; set; }
        public int? UserId { get; set; }
    }
}