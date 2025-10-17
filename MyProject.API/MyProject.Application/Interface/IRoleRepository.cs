using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>> GetAllRolesAsync();
        Task<Roles> GetRoleByIdAsync(Guid id);
        Task<Roles> GetRoleByNameAsync(string name);
        Task AddRoleAsync(Roles role);
        void UpdateRole(Roles role);
        Task DeleteRoleAsync(Guid id);
        void RemoveRangeRole(IEnumerable<Roles> roles);
    }
}
