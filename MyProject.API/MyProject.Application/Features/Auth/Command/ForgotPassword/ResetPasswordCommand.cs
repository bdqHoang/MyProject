using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using System.Text;

namespace MyProject.Application.Features.Auth.Command.FogotPassword
{
    public record ResetPasswordCommand(ResetPasswordReq data) : IRequest<bool>;
    public class ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IRedisService redisService,
        IPasswordHasher<Users> passwordHaser) : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{request.data.Email}:verified"));
            var data = await redisService.GetAsync(key);
            if (string.IsNullOrEmpty(data))
            {
                throw new UnauthorizedAccessException("OTP not verified or expired");
            }

            var user = await userRepository.GetUserByEmailAsync(request.data.Email);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            user.Password = passwordHaser.HashPassword(user, request.data.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await redisService.DeleteAsync(key);
            return await userRepository.UpdateUserAsync(user);
        }
    }
}
