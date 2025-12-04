using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface.Data.Repositories
{
    public interface IDeviceTokenRepository : IBaseRepository<DeviceToken>
    {
        Task<DeviceToken> GetByTokenAsync(string token);
        Task UpdateLastUsedAsync(string token);
    }
}
