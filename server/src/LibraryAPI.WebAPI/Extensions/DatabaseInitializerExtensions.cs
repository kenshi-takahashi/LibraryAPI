using LibraryAPI.Infrastructure.Seed;

namespace LibraryAPI.Extensions
{
    public static class DatabaseInitializerExtensions
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryAPIDbContext>();
                DatabaseInitializer.Initialize(context);
            }
        }
    }
}
