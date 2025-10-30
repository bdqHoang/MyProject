using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext _context) : BaseRepository<Users>(_context), IUserRepository
    {
        /// <summary>
        /// get all users from database
        /// </summary>
        /// <returns></returns>
        override
        public async Task<IEnumerable<Users>> GetAllAsync(int page = 1, int pageSize = 50, CancellationToken ct = default)
        {
            var result = await _dbSet
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.Role)
                .Where(u => u.Status == true)
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// get user by id from database
        /// </summary>
        /// <param name="userId"> Id user</param>
        /// <returns></returns>
        override
        public async Task<Users?> GetByIdAsync(Guid userId, CancellationToken ct = default)
        {
            var result = await _dbSet.Include(u => u.Role)
                .Where(u => u.Status && u.Id == userId).FirstOrDefaultAsync();
            return result!;
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return (await _dbSet.FirstOrDefaultAsync(x => x.Email.Equals(email)))!;
        }

        public async Task<Users> GetUserByPhoneAsync(string phone)
        {
            return (await _dbSet
                .Include(r => r.Role)
                .FirstOrDefaultAsync(x => x.Phone == phone))!;
        }

        /// <summary>
        /// get user by refresh token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Users> GetUserByRefreshTokenAsync(string token)
        {
            return (await _dbSet
                .Include(r=>r.Role)
                .FirstOrDefaultAsync(x => x.RefreshToken == token))!;
        }
    }
}
