using MediatR;
using MyProject.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Message.Command.Update
{
    public record MarkConversationAsReadCommand() : IRequest
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
    public class MarkConversationAsReadCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<MarkConversationAsReadCommand>
    {
        public async Task Handle(MarkConversationAsReadCommand request, CancellationToken cancellationToken)
        {
            var isPaticiant = await _unitOfWork.MessageRepository.IsUserInConversationAsync(request.ConversationId, request.UserId);
            if (!isPaticiant)
            {
                throw new UnauthorizedAccessException("User not author view conversation");
            }
            await _unitOfWork.MessageRepository.MarkMessageAsReadAsync(request.ConversationId, request.UserId);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
