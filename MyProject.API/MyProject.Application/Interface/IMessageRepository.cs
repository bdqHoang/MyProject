using MyProject.Core.Entities;

namespace MyProject.Application.Interface
{
    public interface IMessageRepository
    {
        #region Message operations
        /// <summary>
        /// create a new message in a conversation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task CreateMessageAsync(Messages message);

        /// <summary>
        /// Get message by id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Messages> GetMessageByIdAsync(Guid messageId);

        /// <summary>
        /// get list message in a conversation with pagination
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<Messages>> GetMessagesAsync(Guid conversationId, int page = 1, int pageSize = 50);

        /// <summary>
        ///  mark a message as read by a user
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task MarkMessageAsReadAsync(Guid messageId, Guid userId);

        /// <summary>
        /// delete a message by id (only sender or admin can delete)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteMessageAsync(Guid messageId, Guid userId);
        #endregion

        #region Conversation operations
        /// <summary>
        /// create a new conversation
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        Task CreateConversationAsync(Conversations conversation);

        /// <summary>
        /// get conversation by id
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        Task<Conversations> GetConversationByIdAsync(Guid conversationId);

        /// <summary>
        /// get private conversation between two users (if not exist, return null)
        /// </summary>
        /// <param name="userId1"></param>
        /// <param name="userId2"></param>
        /// <returns></returns>
        Task<Conversations> GetPrivateConversationAsync(Guid userId1, Guid userId2);

        /// <summary>
        /// get list conversations for a user with pagination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// update conversation details (like name, avatar)
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        void UpdateConversation(Conversations conversation);
        #endregion

        #region Participant operations
        /// <summary>
        /// add participant to a conversation
        /// </summary>
        /// <param name="conversationParticipant"></param>
        /// <returns></returns>
        Task AddParticipantAsync(ConversationParticipants conversationParticipant);

        /// <summary>
        /// remove participant from a conversation (only admin can remove)
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveParticipantAsync(Guid conversationId, Guid userId);

        /// <summary>
        /// get list participants in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        Task<IEnumerable<ConversationParticipants>> GetConversationParticipantsAsync(Guid conversationId);

        /// <summary>
        /// check if a user is participant of a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId);

        /// <summary>
        /// update participant role or mute status (only admin can update)
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateLastSeenAsync(Guid conversationId, Guid userId);
        #endregion

        #region Unread count
        /// <summary>
        /// get unread message count for a user in a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId);

        /// <summary>
        /// get all unread message counts for a user across all conversations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Dictionary<Guid, int>> GetAllUnreadMessageCountsAsync(Guid userId);
        #endregion

    }
}
