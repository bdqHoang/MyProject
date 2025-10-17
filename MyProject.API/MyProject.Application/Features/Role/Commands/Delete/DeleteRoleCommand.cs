using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Role.Commands.Delete
{
    public record DeleteRoleCommand(Guid Id): IRequest<bool>;
    public class DeleteRoleCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteRoleCommand, bool>
    {
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.RoleRepository.DeleteRoleAsync(request.Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
