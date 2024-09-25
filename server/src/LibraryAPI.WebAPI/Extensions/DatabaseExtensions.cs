using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LibraryAPIDbContext>(options =>
                options.UseSqlServer(connectionString));
                
            return services;
        }
    }
}
