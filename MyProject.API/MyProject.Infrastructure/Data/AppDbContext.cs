using Microsoft.EntityFrameworkCore;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data.Configurations;

namespace MyProject.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Conversations> Conversations { get; set; }
        public DbSet<ConversationParticipants> ConversationParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationParticipantConfiguration());
        }
    }
}
