using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Infrastructure.Seed
{
    public class DatabaseInitializer
    {
        public static void Initialize(LibraryAPIDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                var roles = new Role[]
                {
                    new Role { Name = "user" },
                    new Role { Name = "admin" }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
