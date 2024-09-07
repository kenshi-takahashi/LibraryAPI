namespace LibraryAPI.Application.DTOs {
    public class UserRegistrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
    }
}