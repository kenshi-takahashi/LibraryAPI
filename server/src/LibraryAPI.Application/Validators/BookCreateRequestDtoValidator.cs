using FluentValidation;
using LibraryAPI.Application.DTOs;

public class BookCreateRequestDtoValidator : AbstractValidator<BookCreateRequestDto>
{
    public BookCreateRequestDtoValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN не может быть пустым.")
            .Length(10, 13).WithMessage("ISBN должен содержать от 10 до 13 символов.");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название не может быть пустым.")
            .MaximumLength(100).WithMessage("Название не может превышать 100 символов.");
        
        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Жанр не может быть пустым.")
            .MaximumLength(50).WithMessage("Жанр не может превышать 50 символов.");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("ID автора должен быть положительным.");
    }
}
