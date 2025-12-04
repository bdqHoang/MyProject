using MyProject.Application.Interface.Data;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; }

        public IMessageRepository MessageRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IConversationRepository ConversationRepository { get; }
        public IParticipantRepository ParticipantRepository { get; }
        public IDeviceTokenRepository DeviceTokenRepository { get; }

        public UnitOfWork(AppDbContext context, 
            IUserRepository userRepository, 
            IMessageRepository messageRepository, 
            IRoleRepository roleRepository,
            IConversationRepository conversationRepository,
            IParticipantRepository participantRepository,
            IDeviceTokenRepository deviceTokenRepository)
        {
            _context = context;
            UserRepository = userRepository;
            MessageRepository = messageRepository;
            RoleRepository = roleRepository;
            ConversationRepository = conversationRepository;
            ParticipantRepository = participantRepository;
            DeviceTokenRepository = deviceTokenRepository;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
