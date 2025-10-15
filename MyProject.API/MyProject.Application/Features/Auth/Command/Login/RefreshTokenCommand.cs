using MediatR;
using Microsoft.Extensions.Options;
using MyProject.Application.Common.Models;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.Login
{
    public record RefreshTokenCommand(string Data) : IRequest<LoginRes>;
    public class RefreshTokenCommandHandler(
        IUserRepository _userRepository,
        ITokenService _tokenService,
        IOptions<JwtSettings> _jwtSettings
        ) : IRequestHandler<RefreshTokenCommand, LoginRes>
    {
        public async Task<LoginRes> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.Data);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            return new LoginRes
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.Name,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationInMinutes)
            };
        }
    }
}
