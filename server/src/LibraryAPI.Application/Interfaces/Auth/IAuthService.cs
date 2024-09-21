using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserRegistrationRequestDto registrationDto);
        Task<AuthResponseDto> Login(UserLoginRequestDto loginDto);
        Task<AuthResponseDto> RefreshToken(TokenRequestDto tokenRequest);
    }
}