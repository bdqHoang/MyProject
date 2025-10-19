using AutoMapper;
using MediatR;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Message.Queries
{
    public record GetMessageQuery : IRequest<List<MessageRes>>
    {
        public Guid ConversationId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public Guid UserId { get; set; }
    }
    public class GetMessageQueryHandler(
        IMessageRepository messageRepository,
        IMapper mapper
        ) : IRequestHandler<GetMessageQuery, List<MessageRes>>
    {
        public async Task<List<MessageRes>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
        {
            // verify user in particiant
            var isPaticiant = await messageRepository.IsUserInConversationAsync(request.ConversationId, request.UserId);
            if (!isPaticiant)
            {
                throw new UnauthorizedAccessException("User not author view conversation");
            }

            // get mesage by conversationid
            var messages = await messageRepository.GetMessagesAsync(request.ConversationId, request.Page, request.PageSize);
            var messageRes = mapper.Map<List<MessageRes>>(messages);

            // update last seen item
            await messageRepository.UpdateLastSeenAsync(request.ConversationId, request.UserId);
            return messageRes;
        }
    }
}
