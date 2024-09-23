using AutoMapper;
using FluentValidation;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

public class BookService : BaseService<Book>, IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    // private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IValidator<BookCreateRequestDto> _createValidator;
    private readonly IValidator<BookUpdateRequestDto> _updateValidator;
    private readonly IValidator<BorrowBookRequestDto> _borrowValidator;
    

    public BookService(IBookRepository bookRepository,
        IMapper mapper, IMemoryCache cache,
        IUserRepository userRepository,
        IValidator<BookCreateRequestDto> createValidator,
        IValidator<BookUpdateRequestDto> updateValidator,
        IValidator<BorrowBookRequestDto> borrowValidator
    // IEmailSender emailService
    ) : base(bookRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        // _emailSender = emailService;
        _mapper = mapper;
        _cache = cache;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _borrowValidator = borrowValidator;
    }

    public async Task<PaginatedResponseDto<BookUserResponseDto>> GetBooksPaginatedAsync(PaginatedRequestDto request)
    {
        var cacheKey = $"PaginatedBooks_{request.PageIndex}_{request.PageSize}";

        return await GetOrAddToCacheAsync(cacheKey, async () =>
        {
            var paginatedList = await GetPaginatedAsync(request.PageIndex, request.PageSize);
            return _mapper.Map<PaginatedResponseDto<BookUserResponseDto>>(paginatedList);
        }, TimeSpan.FromHours(1));
    }

    public async Task<IEnumerable<BookAdminResponseDto>> GetAllBooksAsync()
    {
        var cacheKey = "AllBooks";

        return await GetOrAddToCacheAsync(cacheKey, async () =>
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookAdminResponseDto>>(books);
        }, TimeSpan.FromHours(1));
    }

    public async Task<BookAdminResponseDto> GetBookByIdAsync(int id)
    {
        var cacheKey = $"Book_{id}";

        return await GetOrAddToCacheAsync(cacheKey, async () =>
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return _mapper.Map<BookAdminResponseDto>(book);
        }, TimeSpan.FromHours(1));
    }


    public async Task<IEnumerable<BookAdminResponseDto>> GetBooksByISBNAsync(string isbn)
    {
        var cacheKey = $"BooksByISBN_{isbn}";

        return await GetOrAddToCacheAsync(cacheKey, async () =>
        {
            var books = await _bookRepository.GetByISBNAsync(isbn);
            return _mapper.Map<IEnumerable<BookAdminResponseDto>>(books);
        }, TimeSpan.FromHours(1));
    }


    public async Task AddBookAsync(BookCreateRequestDto bookDto)
    {
        var validationResult = await _createValidator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var book = _mapper.Map<Book>(bookDto);
        await _bookRepository.AddAsync(book);

        if (!string.IsNullOrEmpty(bookDto.Image))
        {
            await AddBookImageAsync(book.Id, bookDto.Image);
        }
    }


    public async Task UpdateBookAsync(int id, BookUpdateRequestDto bookDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var cacheKey = $"Book_{id}";

        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            _mapper.Map(bookDto, book);
            await _bookRepository.UpdateAsync(book);

            _cache.Set(cacheKey, _mapper.Map<BookAdminResponseDto>(book), TimeSpan.FromHours(1));
        }
    }


    public async Task DeleteBookAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            await _bookRepository.DeleteAsync(book);
        }
    }

    public async Task BorrowBookAsync(BorrowBookRequestDto borrowDto)
    {
        var validationResult = await _borrowValidator.ValidateAsync(borrowDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var book = await _bookRepository.GetByIdAsync(borrowDto.BookId);
        if (book != null && book.IsAvailable)
        {
            book.UserId = borrowDto.UserId;
            book.BorrowedAt = DateTime.UtcNow;
            book.ReturnBy = DateTime.UtcNow.AddDays(14);
            book.IsAvailable = false;
            await _bookRepository.UpdateAsync(book);
        }
    }

    public async Task<string> AddBookImageAsync(int bookId, string imagePath)
    {
        var cacheKey = $"BookImage_{bookId}";
        if (_cache.TryGetValue(cacheKey, out string cachedImage))
        {
            return cachedImage;
        }

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book != null)
        {
            book.Image = imagePath;
            await _bookRepository.UpdateAsync(book);
            _cache.Set(cacheKey, imagePath, TimeSpan.FromHours(1));
            return imagePath;
        }

        return null;
    }

    public async Task NotifyUsersAboutReturnDatesAsync()
    {
        // var books = await _bookRepository.GetAllAsync();
        // foreach (var book in books)
        // {
        //     if (book.ReturnBy.HasValue && book.ReturnBy.Value.Date <= DateTime.UtcNow.Date)
        //     {
        //         if (book.UserId.HasValue)
        //         {
        //             var user = await _userRepository.GetByIdAsync(book.UserId.Value);
        //             var userEmail = user.Email;
        //             string subject = "Возврат книги";
        //             string body = $"Пожалуйста, верните книгу '{book.Title}', ваш срок возврата книги {book.ReturnBy.Value.ToShortDateString()}";

        //             await _emailSender.SendEmailAsync(userEmail, subject, body);
        //         }
        //     }
        // }
    }

    private async Task<T> GetOrAddToCacheAsync<T>(string cacheKey, Func<Task<T>> getData, TimeSpan cacheDuration)
    {
        if (_cache.TryGetValue(cacheKey, out T cachedData))
        {
            return cachedData;
        }

        var data = await getData();
        _cache.Set(cacheKey, data, cacheDuration);
        return data;
    }
}
