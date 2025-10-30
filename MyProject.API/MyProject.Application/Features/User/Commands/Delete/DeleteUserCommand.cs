using MediatR;
using MyProject.Application.Interface.Data;

namespace MyProject.Application.Features.User.Commands.Delete
{
    public record DeleteUserCommand(Guid Id) : IRequest<bool>;

    public class DeleteUserCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.UserRepository.Delete(request.Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
