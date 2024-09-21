using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(UserLoginRequestDto loginRequest);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RegisterAsync(UserRegistrationRequestDto registrationRequest);
    }
}