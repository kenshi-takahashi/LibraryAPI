using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
    }
}
