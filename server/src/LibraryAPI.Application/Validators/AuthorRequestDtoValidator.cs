using FluentValidation;
using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.Validators
{
    public class AuthorRequestDtoValidator : AbstractValidator<AuthorRequestDto>
    {
        public AuthorRequestDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.")
                .Length(50).WithMessage("Имя должно содержать до 50 символов.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна для заполнения.")
                .Length(50).WithMessage("Фамилия должна содержать до 50 символов.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна для заполнения.")
                .LessThan(DateTime.UtcNow).WithMessage("Дата рождения должна быть в прошлом.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Страна обязательна для заполнения.")
                .Length(50).WithMessage("Страна должна содержать до 50 символов.");
        }
    }
}
