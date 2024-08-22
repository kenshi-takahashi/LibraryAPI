namespace LibraryAPI.Domain.Entities {
    public class Book : BaseEntity
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? Image { get; set; }
        public int? UserId { get; set; }
        
        // Navigation properties
        public Author Author { get; set; }
        public User? User { get; set; }
    }
}