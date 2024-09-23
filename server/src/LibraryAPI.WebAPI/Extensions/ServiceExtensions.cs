using FluentValidation;
using Hangfire;
using LibraryAPI.Application.Common;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Services;

namespace LibraryAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddHangfire(cfg =>
                cfg.UseSqlServerStorage(configuration["ConnectionStrings:DefaultConnection"]));
            services.AddHangfireServer();

            // Services
            // services.AddSingleton(provider =>
            // {
            //     return GmailServiceInitializer.GetGmailServiceAsync().GetAwaiter().GetResult();
            // });
            // services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            // services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();

            //Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

            return services;
        }
    }
}
