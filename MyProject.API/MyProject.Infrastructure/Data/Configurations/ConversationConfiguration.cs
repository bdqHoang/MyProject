using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversations>
    {
        public void Configure(EntityTypeBuilder<Conversations> builder)
        {
            builder.ToTable("Conversations");
            builder.HasKey(c=>c.Id);
            builder.Property(c => c.GroupName);
            builder.Property(c => c.Avatar);
            builder.Property(c => c.LastMessage);
            builder.Property(c => c.Type);
            builder.Property(c => c.LastMessageAt);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.Status);
        }
    }
}
