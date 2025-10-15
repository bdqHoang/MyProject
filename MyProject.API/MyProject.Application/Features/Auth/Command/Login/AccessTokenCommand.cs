using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyProject.Application.Common.Models;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Auth.Command.Login
{
    public record AccessTokenCommand(LoginReq Data) : IRequest<LoginRes>;
    public class AccessTokenCommandHandler(
        IUserRepository _userRepostitory,
        IPasswordHasher<Users> _passwordHasher,
        ITokenService _tokenService,
        IOptions<JwtSettings> _jwtSettings

        ) : IRequestHandler<AccessTokenCommand, LoginRes>
    {
        public async Task<LoginRes> Handle(AccessTokenCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepostitory.GetUserByEmailAsync(request.Data.Email) ?? throw new UnauthorizedAccessException("Invalid Email or Password");

            if (!user.Status)
            {
                throw new UnauthorizedAccessException("Your account has been locked. Please contact admin to unlock.");
            }

            // verify passwork
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Data.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                user.RetryPassworkCount += 1;
                user.UpdatedAt = DateTime.UtcNow;
                if (user.RetryPassworkCount >= 5)
                {
                    user.Status = false; // lock account
                }
                await _userRepostitory.UpdateUserAsync(user);
                throw new UnauthorizedAccessException($"Incorrect email or password. You still have {5 - user.RetryPassworkCount} attempts left.");
            }

            if (!user.Status)
            {
                throw new UnauthorizedAccessException("Your account has been deativated");
            }

            // Get role user
            var userDetail = await _userRepostitory.GetUserByIdAsync(user.Id);

            if (string.IsNullOrEmpty(userDetail.Role.Name))
            {
                throw new UnauthorizedAccessException("User not role found");
            }

            var accessToken = _tokenService.GenerateAccessToken(userDetail);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RetryPassworkCount = 0;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepostitory.UpdateUserAsync(user);

            return new LoginRes
            {
                Id = userDetail.Id,
                Email = userDetail.Email,
                Name = userDetail.Name,
                Role = userDetail.Role.Name,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationInMinutes)
            };

        }
    }
}
