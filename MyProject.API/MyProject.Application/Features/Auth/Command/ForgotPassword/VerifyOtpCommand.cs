using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.ForgotPassword
{
    public record VerifyOtpCommand : IRequest<bool>
    {
        public string Otp { get; set; } = null!;
        public string Email { get; set; } = null!;
    };
    public class VerifyOtpCommandHandler(IOtpService otpService) : IRequestHandler<VerifyOtpCommand, bool>
    {
        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await otpService.ValidateOtpAsync(request.Email, request.Otp);
        }
    }
}
