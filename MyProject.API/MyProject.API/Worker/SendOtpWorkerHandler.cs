using MediatR;
using MyProject.Application.Features.Auth.Command.ForgotPassword;
using MyProject.Application.Interface.Worker;

namespace MyProject.API.Worker
{
    public class SendOtpWorkerHandler(ISender sender) : IMessageHandler<SendOtpCommand>
    {
        public async Task HandleMessageAsync(SendOtpCommand command, CancellationToken cancellationToken)
        {
            await sender.Send(command);
        }
    }
}
