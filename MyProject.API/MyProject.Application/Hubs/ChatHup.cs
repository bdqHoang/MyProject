using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyProject.Application.Features.Message.Command.Create;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Core.Enum;
using System.Security.Claims;

namespace MyProject.Application.Hubs
{
    [Authorize]
    public class ChatHup(IMessageRepository messageRepository, ISender sender) : Hub
    {
        private string GetUserId() => Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        private string GetUserName() => Context.User.FindFirst(ClaimTypes.Name)?.Value!;
        public override async Task OnConnectedAsync()
        {
            if (!string.IsNullOrEmpty(GetUserId()))
            {
                // add their to a group based on their user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{GetUserId()}");

                // get user conversations
                var conversations = await messageRepository.GetUserConversationsAsync(Guid.Parse(GetUserId()));

                foreach (var conversation in conversations)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversation.Id}");
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (!string.IsNullOrEmpty(GetUserId()))
            {
                // remove their to a group based on their user ID
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{GetUserId()}");
                // get user conversations
                var conversations = await messageRepository.GetUserConversationsAsync(Guid.Parse(GetUserId()));
                foreach (var conversation in conversations)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversation.Id}");
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// send message
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage(Guid? conversationId, Guid reciverId ,string content, MessageType messageType = MessageType.Text)
        {
            try
            {
                var command = new SendMessageCommand
                {
                    ConversationId = conversationId,
                    ReciverId = reciverId,
                    Content = content,
                    Type = (MessageType)messageType,
                    SenderId = Guid.Parse(GetUserId())
                };
                var result = await sender.Send(command);

                // send to all participant in conversation
                await Clients.Group($"conversation_{result.ConversationId}").SendAsync("ReceiveMessage", result);


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task MarkAsRead(Guid conversationId)
        {
            try
            {
                var userId = Guid.Parse(GetUserId());
                var isParticipant = await messageRepository.IsUserInConversationAsync(conversationId, userId);
                if (!isParticipant)
                {
                    await Clients.Caller.SendAsync("Error", "User are not  a participant in this conversation");
                    return;
                }
                await messageRepository.UpdateLastSeenAsync(conversationId, userId);
                await Clients.Caller.SendAsync("MarkedAsRead", conversationId);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task Typing(Guid conversationId, bool isTyping)
        {
            await Clients.OthersInGroup($"conversation_{conversationId}").SendAsync("UserTyping", new
            {
                userId = GetUserId(),
                userName = GetUserName(),
                isTyping = isTyping
            });
        }

        public async Task JoinConversation(Guid conversationId)
        {
            var userId = Guid.Parse(GetUserId());
            // verify user in participant
            var isParticipant = await messageRepository.IsUserInConversationAsync(conversationId, userId);
            if (isParticipant)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
                await messageRepository.UpdateLastSeenAsync(conversationId, userId);
            }
        }

        public async Task LeaverConversation(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }
    }
}
