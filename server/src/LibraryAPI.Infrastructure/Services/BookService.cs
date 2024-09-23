using AutoMapper;
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

    public BookService(IBookRepository bookRepository, 
        IMapper mapper, IMemoryCache cache, 
        IUserRepository userRepository 
        // IEmailSender emailService
    ) : base(bookRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        // _emailSender = emailService;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PaginatedResponseDto<BookUserResponseDto>> GetBooksPaginatedAsync(PaginatedRequestDto request)
    {
        var paginatedList = await GetPaginatedAsync(request.PageIndex, request.PageSize);
        var response = _mapper.Map<PaginatedResponseDto<BookUserResponseDto>>(paginatedList);

        return response;
    }

    public async Task<IEnumerable<BookAdminResponseDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookAdminResponseDto>>(books);
    }

    public async Task<BookAdminResponseDto> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return _mapper.Map<BookAdminResponseDto>(book);
    }

    public async Task<IEnumerable<BookAdminResponseDto>> GetBooksByISBNAsync(string isbn)
    {
        var books = await _bookRepository.GetByISBNAsync(isbn);
        return _mapper.Map<IEnumerable<BookAdminResponseDto>>(books);
    }

    public async Task AddBookAsync(BookCreateRequestDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        await _bookRepository.AddAsync(book);
    }

    public async Task UpdateBookAsync(int id, BookUpdateRequestDto bookDto)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            _mapper.Map(bookDto, book);
            await _bookRepository.UpdateAsync(book);
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

    public async Task BorrowBookAsync(int bookId, int userId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book != null && book.IsAvailable)
        {
            book.UserId = userId;
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
}
