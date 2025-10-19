using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipants>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipants> builder)
        {
            builder.ToTable("ConversationParticipants");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.ConversationId);
            builder.Property(c => c.UserId);
            builder.Property(c => c.Role);
            builder.Property(c => c.IsMuted);
            builder.Property(c => c.JoinedAt);
            builder.Property(c => c.JoinedAt);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.Status);
        }
    }
}
