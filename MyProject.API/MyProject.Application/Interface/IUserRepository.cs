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
        Task AddUserAsync(Users user);
        void UpdateUser(Users user);
        Task DeleteUserAsync(Guid userId);
        Task RemoveRangeUserAsync(IEnumerable<Guid> usersId);
    }
}
