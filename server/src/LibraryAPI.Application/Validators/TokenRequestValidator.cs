using FluentValidation;
using LibraryAPI.Application.DTOs;

public class TokenRequestValidator : AbstractValidator<TokenRequestDto>
{
    public TokenRequestValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}