using AutoMapper;
using MediatR;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.User.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDetailRes>;
    public class GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IMapper _mapper
        ) : IRequestHandler<GetUserByIdQuery, UserDetailRes>
    {
        public async Task<UserDetailRes> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(request.Id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<UserDetailRes>(user);
        }
    }
}
