using MediatR;
using MemoryPack;
using MyProject.Application.Interface.Services;
using MyProject.Application.Interface.Workers;
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
            var body = $@"
                <p>Xin chào,</p>
                <p>Mã OTP đặt lại mật khẩu của bạn là:</p>
                <p><strong>{otp}</strong></p>
                <p>Vui lòng không chia sẻ mã này với bất kỳ ai.</p>
                <p>Trân trọng,<br/>Đội ngũ MyApp</p>
                ";
            return await emailService.SendEmailAsync(request.Email, subject, body);
        }
    }
}
