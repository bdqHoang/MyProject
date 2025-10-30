using MyProject.Core.Entities;

namespace MyProject.Application.Interface.Data.Repositories
{
    public interface IParticipantRepository : IBaseRepository<ConversationParticipants>
    {
        #region Participant operations
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

        /// <summary>
        /// get all unread message counts for a user across all conversations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Dictionary<Guid, int>> GetAllUnreadMessageCountsAsync(Guid userId);
        #endregion
    }
}
