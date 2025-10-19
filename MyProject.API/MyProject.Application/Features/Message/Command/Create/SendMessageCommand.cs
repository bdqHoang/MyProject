using AutoMapper;
using MediatR;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Core.Enum;
using System.Text.Json.Serialization;

namespace MyProject.Application.Features.Message.Command.Create
{
    public record SendMessageCommand : IRequest<MessageRes>
    {
        public Guid? ConversationId { get; set; }
        public Guid? ParrentId { get; set; }
        public Guid ReciverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; }

        [JsonIgnore]
        public Guid SenderId { get; set; }
    };
    public class SendMessageCommandHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<SendMessageCommand, MessageRes>
    {
        public async Task<MessageRes> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                Conversations? conversation;
                // check conversation is exists
                // if exists don't create Conversation
                if (request.ConversationId.HasValue)
                {
                    conversation = await _unitOfWork.MessageRepository.GetConversationByIdAsync(request.ConversationId.Value);
                    if (conversation == null)
                    {
                        throw new KeyNotFoundException("Conversation not found");
                    }

                    // check if sender is in conversation
                    var isParticipant = await _unitOfWork.MessageRepository.IsUserInConversationAsync(conversation.Id, request.SenderId);
                    if (!isParticipant)
                    {
                        throw new UnauthorizedAccessException("You are not a participant of this conversation");
                    }
                }
                else
                {
                    // find private conversation
                    // if not exists create new conversation
                    conversation = await _unitOfWork.MessageRepository.GetPrivateConversationAsync(request.SenderId, request.ReciverId);
                    if (conversation == null)
                    {
                        // create new conversation
                        conversation = new Conversations()
                        {
                            Id = Guid.NewGuid(),
                            Type = ConversationType.Personal,
                            Status = true
                        };

                        await _unitOfWork.MessageRepository.CreateConversationAsync(conversation);

                        // create participants
                        var senderParticipant = new ConversationParticipants()
                        {
                            Id = Guid.NewGuid(),
                            ConversationId = conversation.Id,
                            UserId = request.SenderId,
                            JoinedAt = DateTime.UtcNow,
                            Status = true
                        };
                        await _unitOfWork.MessageRepository.AddParticipantAsync(senderParticipant);

                        // create receiver participant
                        var receiverParticipant = new ConversationParticipants()
                        {
                            Id = Guid.NewGuid(),
                            ConversationId = conversation.Id,
                            UserId = request.ReciverId,
                            JoinedAt = DateTime.UtcNow,
                            Status = true
                        };
                        await _unitOfWork.MessageRepository.AddParticipantAsync(receiverParticipant);
                    }
                }

                // create message
                var message = new Messages()
                {
                    Id = Guid.NewGuid(),
                    ConversationId = conversation.Id,
                    SenderId = request.SenderId,
                    Content = request.Content,
                    Type = request.Type,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = true
                };

                await _unitOfWork.MessageRepository.CreateMessageAsync(message);
                await _unitOfWork.CommitTransactionAsync();


                var messageRes = _mapper.Map<MessageRes>(message);
                message = await _unitOfWork.MessageRepository.GetMessageByIdAsync(message.Id);
                messageRes.SenderName = message.Sender.Name;
                if (!string.IsNullOrEmpty(message.ParrentId.ToString()))
                {
                    var parrentMessage = await _unitOfWork.MessageRepository.GetMessageByIdAsync(message.ParrentId!.Value);
                    if (parrentMessage != null)
                    {
                        messageRes.ParrentName =parrentMessage.Sender!.Name;
                        messageRes.ParrentContent = parrentMessage.Content;
                    }
                }
                return messageRes;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new ArgumentException("Can't send message");
            }
        }
    }
}
