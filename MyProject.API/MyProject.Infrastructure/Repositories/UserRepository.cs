using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.Features.User.DTO;
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
        public async Task<Guid> AddUserAsync(Users user)
        {

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user.Id;
        }

        /// <summary>
        /// update user in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserAsync(Users user)
        {
            dbContext.Users.Update(user);

            return await dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// delete user from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            dbContext.Users.Remove(user!);
            return await dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// remove range user
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> RemoveRangeUserAsync(IEnumerable<Guid> usersId)
        {
            var lstUserRemove = dbContext.Users.Where(x => usersId.Contains(x.Id)).ToList();
            dbContext.Users.RemoveRange(lstUserRemove);
            return await dbContext.SaveChangesAsync() > 0;
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
