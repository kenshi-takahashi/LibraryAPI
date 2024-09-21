using AutoMapper;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Common;

namespace LibraryAPI.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Author mappings
            CreateMap<AuthorRequestDto, Author>();
            CreateMap<Author, AuthorResponseDto>();

            // Book mappings
            CreateMap<BookCreateRequestDto, Book>();
            CreateMap<BookUpdateRequestDto, Book>();
            CreateMap<Book, BookAdminResponseDto>();
            CreateMap<Book, BookUserResponseDto>();

            // User mappings
            CreateMap<UserLoginRequestDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserRegistrationRequestDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserSelfUpdateRequestDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserAdminUpdateRequestDto, User>();
            CreateMap<User, UserAdminResponseDto>();
            CreateMap<User, UserSelfResponseDto>();

            // PaginatedList mapping
            CreateMap<PaginatedList<Author>, PaginatedResponseDto<AuthorResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}