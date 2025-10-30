using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class RoleRepository(AppDbContext _context) : BaseRepository<Roles>(_context), IRoleRepository
    {
        public async Task<Roles> GetRoleByNameAsync(string name)
        {
            return (await _dbSet.FirstOrDefaultAsync(r => r.Name == name))!;
        }
    }
}
