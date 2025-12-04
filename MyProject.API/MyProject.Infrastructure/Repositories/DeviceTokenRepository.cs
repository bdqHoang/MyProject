using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class DeviceTokenRepository(AppDbContext _context) : BaseRepository<DeviceToken>(_context), IDeviceTokenRepository
    {
        public async Task<DeviceToken> GetByTokenAsync(string token)
        {
            return (await _dbSet.FirstOrDefaultAsync(x => x.Token.Equals(token)))!;
        }

        public async Task UpdateLastUsedAsync(string token)
        {
            var result = await _dbSet.FirstOrDefaultAsync(_ => _.Token.Equals(token));
            if (result != null)
            {
                result.Token = token;
                result.UpdateAt = DateTime.Now;
                _dbSet.Update(result);
            }
        }
    }
}
