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

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

            return services;
        }
    }
}
