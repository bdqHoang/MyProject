using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Role.Queries
{
    public record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDetailRes>;
    public class GetRoleByIdQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetRoleByIdQuery, RoleDetailRes>
    {
        public async Task<RoleDetailRes> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(request.RoleId);
            return role == null ? throw new KeyNotFoundException("Role not found") : _mapper.Map<RoleDetailRes>(role);
        }
    }
}
