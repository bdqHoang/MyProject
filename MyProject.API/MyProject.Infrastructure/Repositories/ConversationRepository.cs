using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Core.Enum;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class ConversationRepository(AppDbContext _context) : BaseRepository<Conversations>(_context), IConversationRepository
    {
        public async Task<Conversations> GetPrivateConversationAsync(Guid userId1, Guid userId2)
        {
            var result = await _dbSet
                .Include(c => c.Participants)
                .Where(c => c.Type == ConversationType.Personal && c.Status)
                .Where(c => c.Participants.Any(p => p.UserId == userId1)
                         && c.Participants.Any(p => p.UserId == userId2)
                       ).FirstOrDefaultAsync();
            return result!;
        }

        public async Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId, int page = 1, int pageSize = 20)
        {
            var result = await _dbSet
               .Include(c => c.Participants)
               .ThenInclude(p => p.User)
               .Where(c => c.Participants.Any(p => p.UserId == userId && p.Status) && c.Status)
               .OrderByDescending(c => c.UpdatedAt)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return result;
        }
    }
}
