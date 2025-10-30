using MyProject.Core.Entities;

namespace MyProject.Application.Interface.Data.Repositories
{
    public interface IConversationRepository : IBaseRepository<Conversations>
    {
        #region Conversation operations
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
        #endregion
    }
}
