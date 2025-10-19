using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Core.Enum;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class MessageRepository(AppDbContext dbContext) : IMessageRepository
    {
        /// <summary>
        /// add a participant to a conversation
        /// </summary>
        /// <param name="conversationParticipant"></param>
        /// <returns></returns>
        public async Task AddParticipantAsync(ConversationParticipants conversationParticipant)
        {
            await dbContext.ConversationParticipants.AddAsync(conversationParticipant);
        }

        /// <summary>
        /// create a new conversation
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public async Task CreateConversationAsync(Conversations conversation)
        {
            await dbContext.Conversations.AddAsync(conversation);
        }

        /// <summary>
        /// send a message in a conversation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task CreateMessageAsync(Messages message)
        {
            await dbContext.Messages.AddAsync(message);
        }

        /// <summary>
        /// delete a message by id (only sender can delete)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteMessageAsync(Guid messageId, Guid userId)
        {
            var message = await dbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == userId && m.Status);
            if (message == null)
                return;
            message.Status = false;
            message.UpdatedAt = DateTime.UtcNow;
            dbContext.Messages.Update(message);
        }

        /// <summary>
        /// show unread message count for a user in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId)
        {
            var count = dbContext.Messages
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && m.Status && m.ReadAt == null)
                .CountAsync();
            return count;
        }

        /// <summary>
        /// show unread message counts for a user in all conversations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Dictionary<Guid, int>> GetAllUnreadMessageCountsAsync(Guid userId)
        {
            var conversation = await dbContext.ConversationParticipants
                .Where(cp => cp.UserId == userId).ToListAsync();
            var result = new Dictionary<Guid, int>();
            foreach (var item in conversation)
            {
                var count = await GetUnreadMessageCountAsync(item.ConversationId, userId);
                result[item.ConversationId] = count;
            }
            return result;
        }

        /// <summary>
        /// get conversation by id
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        public async Task<Conversations> GetConversationByIdAsync(Guid conversationId)
        {
            return (await dbContext.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.Status))!;
        }

        /// <summary>
        /// get all participants in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ConversationParticipants>> GetConversationParticipantsAsync(Guid conversationId)
        {
            return await dbContext.ConversationParticipants
                .Where(cp => cp.ConversationId == conversationId && cp.Status)
                .ToListAsync();
        }

        /// <summary>
        /// get message by id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<Messages> GetMessageByIdAsync(Guid messageId)
        {
            return (await dbContext.Messages
                .Include(c=>c.Sender)
                .FirstOrDefaultAsync(m => m.Id == messageId && m.Status))!;
        }

        /// <summary>
        /// get messages in a conversation with pagination
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Messages>> GetMessagesAsync(Guid conversationId, int page = 1, int pageSize = 50)
        {
            var result = await dbContext.Messages
                .Where(m => m.ConversationId == conversationId && m.Status)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// get private conversation between two users (if not exist, return null)
        /// </summary>
        /// <param name="userId1"></param>
        /// <param name="userId2"></param>
        /// <returns></returns>
        public async Task<Conversations> GetPrivateConversationAsync(Guid userId1, Guid userId2)
        {
            var result = await dbContext.Conversations
                .Include(c => c.Participants)
                .Where(c => c.Type == ConversationType.Personal && c.Status)
                .Where(c => c.Participants.Any(p => p.UserId == userId1)
                         && c.Participants.Any(p => p.UserId == userId2)
                       ).FirstOrDefaultAsync();
            return result!;
        }

        /// <summary>
        /// get user's conversations with pagination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId, int page = 1, int pageSize = 20)
        {
            var result = await dbContext.Conversations
                .Include(c=> c.Participants)
                .ThenInclude(p=> p.User)
                .Where(c => c.Participants.Any(p => p.UserId == userId && p.Status) && c.Status)
                .OrderByDescending(c => c.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// check if a user is in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId)
        {
            return await dbContext.ConversationParticipants
                .AnyAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId && cp.Status);
        }

        /// <summary>
        /// mark a message as read by a user
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task MarkMessageAsReadAsync(Guid messageId, Guid userId)
        {
            var message = await dbContext.Messages.FindAsync(messageId);
            if (message == null || message.SenderId == userId || !message.Status || message.ReadAt != null)
                return;
            message.ReadAt = DateTime.UtcNow;
            dbContext.Messages.Update(message);
        }

        /// <summary>
        /// remove a participant from a conversation (soft delete)
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RemoveParticipantAsync(Guid conversationId, Guid userId)
        {
            var participant = await dbContext.ConversationParticipants
                .FirstOrDefaultAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId && cp.Status);
            if (participant == null)   return;

            participant.Status = false;
            participant.UpdatedAt = DateTime.UtcNow;
            dbContext.ConversationParticipants.Update(participant);
        }

        /// <summary>
        /// update conversation details
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public void UpdateConversation(Conversations conversation)
        {
            dbContext.Conversations.Update(conversation);
        }

        /// <summary>
        /// update last seen time for a user in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateLastSeenAsync(Guid conversationId, Guid userId)
        {
            var participant = await dbContext.ConversationParticipants
                .FirstOrDefaultAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId && cp.Status);
            if (participant == null) return;

            participant.UpdatedAt = DateTime.UtcNow;
            dbContext.ConversationParticipants.Update(participant);
        }
    }
}
