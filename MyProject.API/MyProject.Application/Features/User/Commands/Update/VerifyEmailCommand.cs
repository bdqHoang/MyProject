using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Commands.Update
{
    public record VerifyEmailCommand(string Token) : IRequest<bool>;
    public class VerifyEmailCommandHandler(IUserRepository userRepository, ITokenService tokenService) : IRequestHandler<VerifyEmailCommand, bool>
    {
        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var isValid = tokenService.ValidateEmailToken(request.Token, out var email);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid or expired token.");
            }

            var user = await userRepository.GetUserByEmailAsync(email) 
                ?? throw new UnauthorizedAccessException("User not found.");

            user.IsValidEmail = true;
            user.UpdatedAt = DateTime.UtcNow;
            await userRepository.UpdateUserAsync(user);
            return isValid;
        }
    }
}
