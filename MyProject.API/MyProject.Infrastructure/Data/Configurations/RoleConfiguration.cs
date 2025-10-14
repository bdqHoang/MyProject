using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;
using MyProject.Core.Enum;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.Status).IsRequired();

            // Seed initial data
            var adminRoleId = new Guid("00000000-0000-0000-0000-000000000001");
            var userRoleId = new Guid("00000000-0000-0000-0000-000000000002");
            var managerRoleId = new Guid("00000000-0000-0000-0000-000000000003");
            var guestRoleId = new Guid("00000000-0000-0000-0000-000000000004");

            builder.HasData(
                new Roles
                {
                    Id = adminRoleId,
                    Name = RoleName.Admin,
                    Description = "Administrator with full access.",
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true
                },
                new Roles
                {
                    Id = userRoleId,
                    Name = RoleName.User,
                    Description = "Regular user with limited access.",
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true
                },
                new Roles
                {
                    Id = managerRoleId,
                    Name = RoleName.Manager,
                    Description = "Manager with elevated access.",
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true
                },
                new Roles
                {
                    Id = guestRoleId,
                    Name = RoleName.Guest,
                    Description = "Guest user with minimal access.",
                    CreatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = true
                }
            );
        }
    }
}
