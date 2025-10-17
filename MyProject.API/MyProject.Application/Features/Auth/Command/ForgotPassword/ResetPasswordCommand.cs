using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using System.Text;

namespace MyProject.Application.Features.Auth.Command.ForgotPassword
{
    public record ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!;
    };
    public class ChangePasswordCommandHandler(
        IUnitOfWork _unitOfWork,
        IRedisService redisService,
        IPasswordHasher<Users> passwordHaser) : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{request.Email}:verified"));
            var data = await redisService.GetAsync(key);
            if (string.IsNullOrEmpty(data))
            {
                throw new UnauthorizedAccessException("OTP not verified or expired");
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException("User not found");
            user.Password = passwordHaser.HashPassword(user, request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await redisService.DeleteAsync(key);
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
