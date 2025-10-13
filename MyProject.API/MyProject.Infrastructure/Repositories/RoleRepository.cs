using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class RoleRepository(AppDbContext dbContext) : IRoleRepository
    {
        /// <summary>
        /// add new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Guid> AddRoleAsync(Roles role)
        {
            await dbContext.Roles.AddAsync(role);
            await dbContext.SaveChangesAsync();

            return role.Id;
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await this.GetRoleByIdAsync(id);
            dbContext.Roles.Remove(role);

            return await dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// gell all role from databse
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Roles>> GetAllRolesAsync()
        {
            return await dbContext.Roles.ToListAsync();
        }

        /// <summary>
        /// get role from id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Roles> GetRoleByIdAsync(Guid id)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// get role by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Roles> GetRoleByNameAsync(string name)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        /// <summary>
        /// update role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task UpdateRoleAsync(Roles role)
        {
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// reomove range role
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> RemoveRangeRoleAsync(IEnumerable<Roles> roles)
        {
            dbContext.Roles.RemoveRange(roles);
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
