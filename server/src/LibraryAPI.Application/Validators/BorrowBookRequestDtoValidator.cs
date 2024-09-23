using FluentValidation;
using LibraryAPI.Application.DTOs;

public class BorrowBookRequestDtoValidator : AbstractValidator<BorrowBookRequestDto>
{
    public BorrowBookRequestDtoValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("BookId не может быть пустым.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId не может быть пустым.");
    }
}
