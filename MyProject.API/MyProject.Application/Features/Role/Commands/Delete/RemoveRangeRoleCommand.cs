using MediatR;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Role.Commands.Delete
{
    public record RemoveRangeRoleCommand(IEnumerable<Guid> data) : IRequest<bool>;
    public class RemoveRangeRoleCommandHandler(
        IRoleRepository _roleRepository
        ) : IRequestHandler<RemoveRangeRoleCommand, bool>
    {
        public async Task<bool> Handle(RemoveRangeRoleCommand request, CancellationToken cancellationToken)
        {
            var lstRole = await _roleRepository.GetAllRolesAsync();
            var rolesToRemove = lstRole.Where(r => request.data.Contains(r.Id)).ToList();
            if (!rolesToRemove.Any())
            {
                throw new KeyNotFoundException("No roles found for the provided IDs.");
            }
            return await _roleRepository.RemoveRangeRoleAsync(rolesToRemove);
        }
    }
}
