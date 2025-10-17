using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        /// <summary>
        /// get all users from database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            var result = await dbContext.Users
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
        public async Task<Users> GetUserByIdAsync(Guid userId)
        {
            var result = await dbContext.Users.Include(u => u.Role)
                .Where(u=>u.Status && u.Id == userId).FirstOrDefaultAsync();
            return result!;
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return (await dbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email)))!;
        }

        /// <summary>
        /// Add user to database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUserAsync(Users user)
        {
            await dbContext.Users.AddAsync(user);
        }

        /// <summary>
        /// update user in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void UpdateUser(Users user)
        {
            dbContext.Users.Update(user);
        }

        /// <summary>
        /// delete user from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            dbContext.Users.Remove(user!);
        }

        /// <summary>
        /// remove range user
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RemoveRangeUserAsync(IEnumerable<Guid> usersId)
        {
            var lstUserRemove = await dbContext.Users.Where(x => usersId.Contains(x.Id)).ToListAsync();
            dbContext.Users.RemoveRange(lstUserRemove);
        }

        public async Task<Users> GetUserByPhoneAsync(string phone)
        {
            return (await dbContext.Users.FirstOrDefaultAsync(x => x.Phone == phone))!;
        }

        /// <summary>
        /// get user by refresh token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Users> GetUserByRefreshTokenAsync(string token)
        {
            return (await dbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == token))!;
        }
    }
}
