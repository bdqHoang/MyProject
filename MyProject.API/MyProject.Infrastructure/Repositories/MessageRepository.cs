using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class MessageRepository(AppDbContext _context) : BaseRepository<Messages>(_context), IMessageRepository
    {
        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        override
        public async Task<Messages?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.Include(u => u.Sender).FirstAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// get messages in a conversation with pagination
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Messages>> GetMessagesByConversationAsync(Guid conversationId, int page = 1, int pageSize = 50)
        {
            var result = await _dbSet
                .Where(m => m.ConversationId == conversationId && m.Status)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// mark a message as read by a user
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task MarkMessageAsReadAsync(Guid conversationId, Guid userId)
        {
            var messagelst = await _dbSet.Where(x => x.ConversationId == conversationId).ToListAsync();
            var message = messagelst[messagelst.Count];

            if (message == null || message.SenderId == userId || !message.Status || message.ReadAt != null)
                return;
            message.ReadAt = DateTime.UtcNow;
            _dbSet.Update(message);
        }
        /// <summary>
        /// delete a message by id (only sender can delete)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteMessageByUserAsync(Guid messageId, Guid userId)
        {
            var message = await _dbSet.FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == userId && m.Status);
            if (message == null)
                return;
            message.Status = false;
            message.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(message);
        }

        /// <summary>
        /// show unread message count for a user in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId)
        {
            var count = await _dbSet
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && m.Status && m.ReadAt == null)
                .CountAsync();
            return count;
        }
    }
}
