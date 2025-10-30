using MediatR;
using Microsoft.AspNetCore.SignalR;
using MyProject.API.Hubs;
using MyProject.Application.Features.Message.Command.Create;
using MyProject.Application.Interface.Workers;

namespace MyProject.API.Worker
{
    public class ChatMessageWorkerHandler(
        ISender sender,
        IHubContext<ChatHub> hubContext
        ) : IMessageHandler<SendMessageCommand>
    {
        public async Task HandleMessageAsync(SendMessageCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command);

            await hubContext.Clients
                .Group($"conversation_{result.ConversationId}")
                .SendAsync("ReceiveMessage", result);

        }
    }
}
