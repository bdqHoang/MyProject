using MediatR;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.User.Queries
{
    public record GetUserByPhoneQuery(string data): IRequest<Users>;
    public class GetUserByPhoneQueryHandler(IUserRepository _userRepository) : IRequestHandler<GetUserByPhoneQuery, Users>
    {
        public Task<Users> Handle(GetUserByPhoneQuery request, CancellationToken cancellationToken)
        {
            return _userRepository.GetUserByPhoneAsync(request.data);
        }
    }
}
