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
    public record GetAllRoleQuery() : IRequest<IEnumerable<RoleDetailRes>>;
    public class GetAllRoleQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleDetailRes>>
    {
        public async Task<IEnumerable<RoleDetailRes>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleDetailRes>>(roles);
        }
    }
}
