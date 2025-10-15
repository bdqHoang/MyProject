using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Phone).HasMaxLength(15);
            builder.Property(x => x.Avatar).HasMaxLength(255);
            builder.Property(x => x.IsValidEmail).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.RetryPassworkCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.RefreshToken);
            builder.Property(x => x.RefreshTokenExpiryTime);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            // Seed initial data
            var adminRoleId = new Guid("00000000-0000-0000-0000-000000000001");
            var userRoleId = new Guid("00000000-0000-0000-0000-000000000002");

            var adminUserId = new Guid("00000000-0000-0000-0000-000000000001");
            var userUserId = new Guid("00000000-0000-0000-0000-000000000002");

            builder.HasData(
                new Users
                {
                    Id = adminUserId,
                    Name = "Admin User",
                    Email = "admin@gmail.com",
                    Phone = "0123456789",
                    Avatar = "https://example.com/avatar/admin.png",
                    Password = "AQAAAAIAAYagAAAAEPjtQPE0/wVQiUxiJkL7BTRQXPd2u2UD96qrZviV4jt2KzF80u4QQ8GahtqGpKOMvg==",
                    IsValidEmail = false,
                    RoleId = adminRoleId,
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true,
                },
                new Users
                {
                    Id = userUserId,
                    Name = "Normal User",
                    Email = "user@gmail.com",
                    Phone = "0987654321",
                    Password = "AQAAAAIAAYagAAAAEPjtQPE0/wVQiUxiJkL7BTRQXPd2u2UD96qrZviV4jt2KzF80u4QQ8GahtqGpKOMvg==",
                    Avatar = "https://example.com/avatar/user.png",
                    IsValidEmail = true,
                    RoleId = userRoleId,
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true
                }
                );
        }
    }
}
