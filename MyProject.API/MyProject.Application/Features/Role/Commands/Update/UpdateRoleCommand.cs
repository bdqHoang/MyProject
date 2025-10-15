using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Role.Commands.Update
{
    public record UpdateRoleCommand : IRequest<RoleDetailRes>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    };
    public class UpdateRoleCommandHandler(
        IRoleRepository _roleRepository,
        IMapper _mapper
        ) : IRequestHandler<UpdateRoleCommand, RoleDetailRes>
    {
        public async Task<RoleDetailRes> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await _roleRepository.GetRoleByIdAsync(request.Id) ?? throw new KeyNotFoundException("Role not found");
            _mapper.Map(request,existingRole);
            existingRole.UpdatedAt = DateTime.UtcNow;
            
            await _roleRepository.UpdateRoleAsync(existingRole);

            return _mapper.Map<RoleDetailRes>(existingRole);
        }
    }
}
