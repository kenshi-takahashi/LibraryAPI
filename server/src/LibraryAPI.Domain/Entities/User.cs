namespace LibraryAPI.Domain.Entities {
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public int? RoleId { get; set; }
        
        // Navigation properties
        public Role? Role { get; set; }
        public ICollection<Book> BorrowedBooks { get; set; } = new HashSet<Book>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}