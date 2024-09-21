using FluentValidation;
using LibraryAPI.Application.DTOs;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequestDto>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}