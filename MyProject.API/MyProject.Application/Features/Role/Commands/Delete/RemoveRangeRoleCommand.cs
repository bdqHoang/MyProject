using MediatR;
using MyProject.Application.Interface.Data;

namespace MyProject.Application.Features.Role.Commands.Delete
{
    public record RemoveRangeRoleCommand(IEnumerable<Guid> data) : IRequest<bool>;
    public class RemoveRangeRoleCommandHandler(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<RemoveRangeRoleCommand, bool>
    {
        public async Task<bool> Handle(RemoveRangeRoleCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.RoleRepository.RemoveRange(request.data);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
