using MyProject.Application.Interface;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; }

        public IMessageRepository MessageRepository { get; }
        public IRoleRepository RoleRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            MessageRepository = new MessageRepository(context);
            RoleRepository = new RoleRepository(context);
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
