using MediatR;
using MyProject.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.User.Commands.Update
{
    public record VerifyEmailCommand(string token) : IRequest<string>;
    public class VerifyEmailCommandHandler(IUserRepository userRepository, ITokenService tokenService) : IRequestHandler<VerifyEmailCommand, string>
    {
        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var isValid = tokenService.ValidateEmailToken(request.token, out var email);
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            user.IsValidEmail = true;
            user.UpdatedAt = DateTime.UtcNow;
            await userRepository.UpdateUserAsync(user);
            return email;
        }
    }
}
