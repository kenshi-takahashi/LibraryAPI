namespace LibraryAPI.Application.DTOs
{
    public class TokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}