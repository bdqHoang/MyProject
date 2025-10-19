using AutoMapper;
using MediatR;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Message.Queries
{
    public record GetUserConversationsQuery : IRequest<List<ConversationRes>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
    public class GetUserConversationsQueryHandler(
        IMessageRepository messageRepository,
        IMapper mapper
        ) : IRequestHandler<GetUserConversationsQuery, List<ConversationRes>>
    {
        public async Task<List<ConversationRes>> Handle(GetUserConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await messageRepository.GetUserConversationsAsync(request.UserId, request.Page, request.PageSize);
            var conversationUnRead = await messageRepository.GetAllUnreadMessageCountsAsync(request.UserId);

            var conversationRes = mapper.Map<IEnumerable<ConversationRes>>(conversations);
            foreach (var item in conversationRes)
            {
                item.UnReadCount = conversationUnRead.ContainsKey(item.Id)? conversationUnRead[item.Id] : 0;
            }

            return conversationRes.ToList();
        }
    }
}
