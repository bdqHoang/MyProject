using MediatR;
using MemoryPack;
using MyProject.Application.Interface;
using MyProject.Application.Interface.Worker;
using System.Text.Json.Serialization;

namespace MyProject.Application.Features.Auth.Command.ForgotPassword
{
    [MemoryPackable]
    public partial record SendOtpCommand(string Email) : IRequest<bool>, IQueueableMessage
    {
        [JsonIgnore]
        public string? StreamMessageId { get; set; }
    }

    public class SendOtpCommandHandler(
        IOtpService otpService,
        IEmailService emailService
        ) : IRequestHandler<SendOtpCommand, bool>
    {
        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var otp = await otpService.GenerateOtpAsync(request.Email);
            var subject = "Reset Password by MyApp";
            var body = $"Your Otp reset is {otp}";
            return await emailService.SendEmailAsync(request.Email, subject, body);
        }
    }
}
