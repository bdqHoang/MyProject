using MediatR;
using MyProject.Application.Interface.Data;

namespace MyProject.Application.Features.User.Commands.Delete
{
    public record RemoveRangeUserCommand(IEnumerable<Guid> data) : IRequest<bool>;
    public class RemoveRangeUserCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveRangeUserCommand, bool>
    {
        public async Task<bool> Handle(RemoveRangeUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.UserRepository.RemoveRange(request.data);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
