namespace LibraryAPI.Domain.Entities {
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        
        // Navigation property
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}