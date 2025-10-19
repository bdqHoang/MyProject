using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content)
                .HasMaxLength(10000);
            builder.Property(c => c.ConversationId);
            builder.Property(c => c.SenderId);
            builder.Property(c => c.ParrentId);
            builder.Property(c => c.Title);
            builder.Property(c => c.Type);
            builder.Property(c => c.ReadAt);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.Status);
        }
    }
}
