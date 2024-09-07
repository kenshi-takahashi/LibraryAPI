namespace LibraryAPI.Application.DTOs {
    public class BookCreateRequestDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public string? Image { get; set; }
    }
}