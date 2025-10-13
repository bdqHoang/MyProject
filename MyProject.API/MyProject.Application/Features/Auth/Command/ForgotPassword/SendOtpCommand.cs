using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.FogotPassword
{
    public record SendOtpCommand(string data): IRequest<bool>;
    public class SendOtpCommandHandler(
        IOtpService otpService,
        IEmailService emailService
        ) : IRequestHandler<SendOtpCommand, bool>
    {
        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var otp = await otpService.GenerateOtpAsync(request.data);
            var subject = "Reset Password by MyApp";
            var body = $"Your Otp reset is {otp}";
            return await emailService.SendEmailAsync(request.data, subject, body);
        }
    }
}
