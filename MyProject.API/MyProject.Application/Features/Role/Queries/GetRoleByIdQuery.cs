using AutoMapper;
using MediatR;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Interface;
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
            var role = await _unitOfWork.RoleRepository.GetRoleByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }
            return _mapper.Map<RoleDetailRes>(role);
        }
    }
}
