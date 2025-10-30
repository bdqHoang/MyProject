using MyProject.Core.Entities;

namespace MyProject.Application.Interface.Data.Repositories
{
    public interface IMessageRepository : IBaseRepository<Messages>
    {
        /// <summary>
        /// get list message in a conversation with pagination
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<Messages>> GetMessagesByConversationAsync(Guid conversationId, int page = 1, int pageSize = 50);

        /// <summary>
        ///  mark a message as read by a user
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task MarkMessageAsReadAsync(Guid conversationId, Guid userId);

        /// <summary>
        /// delete a message by id (only sender or admin can delete)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteMessageByUserAsync(Guid messageId, Guid userId);

        /// <summary>
        /// get unread message count for a user in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId);
    }
}
