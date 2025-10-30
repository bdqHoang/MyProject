using MyProject.Application.Interface.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
