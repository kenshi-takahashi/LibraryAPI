using FluentValidation;
using LibraryAPI.Application.DTOs;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequestDto>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}