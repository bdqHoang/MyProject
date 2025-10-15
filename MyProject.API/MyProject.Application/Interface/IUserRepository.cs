using MyProject.Application.Features.User.DTO;
using MyProject.Core.Entities;

namespace MyProject.Application.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(Guid userId);
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByPhoneAsync(string phone);
        Task<Users> GetUserByRefreshTokenAsync(string token);
        Task<Guid> AddUserAsync(Users user);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> RemoveRangeUserAsync(IEnumerable<Guid> usersId);
    }
}
