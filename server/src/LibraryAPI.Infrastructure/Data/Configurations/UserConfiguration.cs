using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryAPI.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.MiddleName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.OwnsOne(u => u.RefreshToken, rt =>
        {
            rt.Property(r => r.Token).HasMaxLength(500);
            rt.Property(r => r.ExpirationDate).HasColumnType("datetime2");
        });

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(u => u.BorrowedBooks)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
