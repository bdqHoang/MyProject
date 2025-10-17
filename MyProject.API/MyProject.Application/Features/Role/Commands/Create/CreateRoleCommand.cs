using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Role.Commands.Create
{
    public record CreateRoleCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    };
    public class CreateRoleCommandHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IRequestHandler<CreateRoleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Roles>(request);
            role.Id = Guid.NewGuid();
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;
            role.Status = true;

            await _unitOfWork.RoleRepository.AddRoleAsync(role);
            await _unitOfWork.CommitAsync();
            return role.Id;
        }
    }
}
