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
    public class DeleteRoleCommandHandler(IRoleRepository _roleRepository) : IRequestHandler<DeleteRoleCommand, bool>
    {
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            return await _roleRepository.DeleteRoleAsync(request.Id);
        }
    }
}
