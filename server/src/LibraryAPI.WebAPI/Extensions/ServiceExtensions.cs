using FluentValidation;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Services;

namespace LibraryAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // Services
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Validators
            services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

            return services;
        }
    }
}
