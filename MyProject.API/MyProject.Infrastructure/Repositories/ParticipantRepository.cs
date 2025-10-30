using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class ParticipantRepository(
        AppDbContext _context, 
        IMessageRepository _messageRepository
        ): BaseRepository<ConversationParticipants>(_context), IParticipantRepository
    {
        public async Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId)
        {
            return await _dbSet
                .AnyAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId && cp.Status);
        }

        public async Task UpdateLastSeenAsync(Guid conversationId, Guid userId)
        {
            var participant = await _dbSet
               .FirstOrDefaultAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId && cp.Status);
            if (participant == null) return;

            participant.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(participant);
        }

        /// <summary>
        /// show unread message counts for a user in all conversations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Dictionary<Guid, int>> GetAllUnreadMessageCountsAsync(Guid userId)
        {
            var conversation = await _dbSet
                .Where(cp => cp.UserId == userId).ToListAsync();
            var result = new Dictionary<Guid, int>();
            foreach (var item in conversation)
            {
                var count = await _messageRepository.GetUnreadMessageCountAsync(item.ConversationId, userId);
                result[item.ConversationId] = count;
            }
            return result;
        }

    }
}
