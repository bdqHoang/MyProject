using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Commands.Update
{
    public record VerifyEmailCommand(string Token) : IRequest<bool>;
    public class VerifyEmailCommandHandler(IUnitOfWork _unitOfWork, ITokenService tokenService) : IRequestHandler<VerifyEmailCommand, bool>
    {
        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var isValid = tokenService.ValidateEmailToken(request.Token, out var email);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid or expired token.");
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email) 
                ?? throw new UnauthorizedAccessException("User not found.");

            user.IsValidEmail = true;
            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.CommitAsync();

            return isValid;
        }
    }
}
