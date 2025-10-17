using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Commands.Delete
{
    public record RemoveRangeUserCommand(IEnumerable<Guid> data) : IRequest<bool>;
    public class RemoveRangeUserCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveRangeUserCommand, bool>
    {
        public async Task<bool> Handle(RemoveRangeUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.RemoveRangeUserAsync(request.data);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
