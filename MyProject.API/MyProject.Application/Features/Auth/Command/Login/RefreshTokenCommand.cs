using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MyProject.Application.Common.Models;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.Login
{
    public record RefreshTokenCommand(string Data) : IRequest<LoginRes>;
    public class RefreshTokenCommandHandler(
        IUnitOfWork _unitOfWork,
        ITokenService _tokenService,
        IOptions<JwtSettings> _jwtSettings,
        IMapper _mapper
        ) : IRequestHandler<RefreshTokenCommand, LoginRes>
    {
        public async Task<LoginRes> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByRefreshTokenAsync(request.Data);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);
            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.CommitAsync();

            var logginRes = _mapper.Map<LoginRes>(user);
            logginRes.AccessToken = newAccessToken;
            logginRes.RefreshToken = newRefreshToken;
            logginRes.ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationInMinutes);

            return logginRes;
        }
    }
}
