using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Message.Queries
{
    public record IsUserInConversationQuery(): IRequest<bool>
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
    public class IsUserInConversationQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<IsUserInConversationQuery, bool>
    {
        public async Task<bool> Handle(IsUserInConversationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.IsUserInConversationAsync(request.ConversationId, request.UserId);
        }
    }
}
