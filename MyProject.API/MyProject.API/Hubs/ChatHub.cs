using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyProject.Application.Features.Message.Command.Create;
using MyProject.Application.Features.Message.Command.Update;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Features.Message.Queries;
using MyProject.Application.Interface;
using MyProject.Core.Enum;
using System.Security.Claims;

namespace MyProject.API.Hubs
{
    [Authorize]
    public class ChatHub(IUnitOfWork unitOfWork, ISender sender, IMessageQueueService messageQueueService) : Hub
    {
        private string GetUserId() => Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        private string GetUserName() => Context.User!.FindFirst(ClaimTypes.Name)!.Value!;
        public override async Task OnConnectedAsync()
        {
            if (!string.IsNullOrEmpty(GetUserId()))
            {
                // get user conversations
                var query = new GetUserConversationsQuery
                {
                    UserId = Guid.Parse(GetUserId())
                };

                var result = await sender.Send(query);

                // add their to a group based on their user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{GetUserId()}");

                foreach (var conversation in result)
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
                // get user conversations
                var query = new GetUserConversationsQuery
                {
                    UserId = Guid.Parse(GetUserId())
                };

                var result = await sender.Send(query);
                // remove their to a group based on their user ID
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{GetUserId()}");

                foreach (var conversation in result)
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
        public async Task SendMessage(Guid? conversationId, Guid reciverId, string content, MessageType messageType = MessageType.Text)
        {
            try
            {
                var queue = new QueuedMessageDto
                {
                    ConversationId = conversationId,
                    ReciverId = reciverId,
                    Content = content,
                    Type = messageType,
                    SenderId = Guid.Parse(GetUserId()),
                    QueueAt = DateTime.UtcNow,
                    RetryCount = 0,
                    Status  = MessageStatus.Pending
                };

                await messageQueueService.PublishMessageAsync(queue);

                // send to all participant in conversation
                await Clients.Caller.SendAsync("MessageQueued", queue);


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
                var query = new IsUserInConversationQuery
                {
                    ConversationId = conversationId,
                    UserId = Guid.Parse(GetUserId())
                };
                var isParticipant = await sender.Send(query);
                if (!isParticipant)
                {
                    await Clients.Caller.SendAsync("Error", "User are not  a participant in this conversation");
                    return;
                }

                var markAsReadCommand = new MarkConversationAsReadCommand
                {
                    ConversationId = conversationId,
                    UserId = Guid.Parse(GetUserId())
                };
                await sender.Send(markAsReadCommand);

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
            var query = new IsUserInConversationQuery
            {
                ConversationId = conversationId,
                UserId = userId
            };
            var isParticipant = await sender.Send(query);
            if (isParticipant)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
                await unitOfWork.MessageRepository.UpdateLastSeenAsync(conversationId, userId);
                await unitOfWork.CommitAsync();
            }
        }

        public async Task LeaverConversation(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }
    }
}
