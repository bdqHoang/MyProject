using MyProject.Application.Features.User.DTO;
using MyProject.Core.Entities;

namespace MyProject.Application.Interface.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<Users>
    {
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByPhoneAsync(string phone);
        Task<Users> GetUserByRefreshTokenAsync(string token);
    }
}
