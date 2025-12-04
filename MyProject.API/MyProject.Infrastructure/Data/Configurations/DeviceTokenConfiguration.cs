using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class DeviceTokenConfiguration : IEntityTypeConfiguration<DeviceToken>
    {
        public void Configure(EntityTypeBuilder<DeviceToken> builder)
        {
            builder.ToTable("DeviceTokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId);
            builder.Property(x => x.Token);
            builder.Property(x => x.DeviceType);
            builder.Property(x => x.DeviceInfo);
            builder.Property(x => x.CreateAt);
            builder.Property(x => x.UpdateAt);
            builder.Property(x => x.Status);
        }
    }
}
