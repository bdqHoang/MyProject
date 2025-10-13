using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Role.Commands.Create
{
    public record CreateRoleCommand(CreateRoleReq data) : IRequest<Guid>;
    public class CreateRoleCommandHandler(
        IRoleRepository _roleRepository,
        IMapper _mapper) : IRequestHandler<CreateRoleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Roles>(request.data);
            role.Id = Guid.NewGuid();
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;
            role.Status = true;

            return await _roleRepository.AddRoleAsync(role);
        }
    }
}
