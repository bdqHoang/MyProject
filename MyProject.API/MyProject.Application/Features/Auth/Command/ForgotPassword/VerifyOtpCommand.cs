using MediatR;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.FogotPassword
{
    public record VerifyOtpCommand(VerifyOtpReq data) : IRequest<bool>;
    public class VerifyOtpCommandHandler(IOtpService otpService) : IRequestHandler<VerifyOtpCommand, bool>
    {
        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await otpService.ValidateOtpAsync(request.data.Email, request.data.Otp);
        }
    }
}
