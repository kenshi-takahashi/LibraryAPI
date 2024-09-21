using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Entities;
using FluentValidation;
using System.Security.Claims;
using LibraryAPI.Application.Interfaces;

namespace LibraryAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserRegistrationRequestDto> _registrationValidator;
        private readonly IValidator<UserLoginRequestDto> _loginValidator;
        private readonly IValidator<TokenRequestDto> _tokenRequestValidator;

        public AuthService(
            ITokenService tokenService,
            IUserRepository userRepository,
            IValidator<UserRegistrationRequestDto> registrationValidator,
            IValidator<UserLoginRequestDto> loginValidator,
            IValidator<TokenRequestDto> tokenRequestValidator)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _registrationValidator = registrationValidator;
            _loginValidator = loginValidator;
            _tokenRequestValidator = tokenRequestValidator;
        }

        public async Task<string> Register(UserRegistrationRequestDto registrationDto)
        {
            var validationResult = await _registrationValidator.ValidateAsync(registrationDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var existingUser = await _userRepository.GetByEmailAsync(registrationDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Email = registrationDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password),
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                MiddleName = registrationDto.MiddleName,
                RoleId = 2
            };

            await _userRepository.AddAsync(user);
            return "User registered successfully.";
        }

        public async Task<AuthResponseDto> Login(UserLoginRequestDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new Exception("Invalid email or password.");
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = new RefreshToken
            {
                Token = refreshToken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = DateTime.Now.AddMinutes(30)
            };
        }

        public async Task<AuthResponseDto> RefreshToken(TokenRequestDto tokenRequest)
        {
            var validationResult = await _tokenRequestValidator.ValidateAsync(tokenRequest);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            var email = principal.Identity.Name;

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.RefreshToken == null || user.RefreshToken.Token != tokenRequest.RefreshToken ||
                user.RefreshToken.ExpirationDate <= DateTime.Now)
            {
                throw new Exception("Invalid refresh token.");
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);

            user.RefreshToken = new RefreshToken
            {
                Token = newRefreshToken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiration = DateTime.Now.AddMinutes(30)
            };
        }
    }
}