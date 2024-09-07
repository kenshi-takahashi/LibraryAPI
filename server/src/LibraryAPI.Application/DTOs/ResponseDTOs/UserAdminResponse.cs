namespace LibraryAPI.Application.DTOs
{
    public class UserAdminResponseDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int RoleId { get; set; }
    }
}