using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Role.Commands.Update
{
    public record UpdateRoleCommand( Guid id, UpdateRoleReq data) : IRequest<RoleDetailRes>;
    public class UpdateRoleCommandHandler(
        IRoleRepository _roleRepository,
        IMapper _mapper
        ) : IRequestHandler<UpdateRoleCommand, RoleDetailRes>
    {
        public async Task<RoleDetailRes> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {

            var existingRole = await _roleRepository.GetRoleByIdAsync(request.id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            existingRole = _mapper.Map<Roles>(request.data);
            existingRole.Id = Guid.NewGuid();
            existingRole.UpdatedAt = DateTime.UtcNow;
            
            await _roleRepository.UpdateRoleAsync(existingRole);

            return _mapper.Map<RoleDetailRes>(existingRole);
        }
    }
}
