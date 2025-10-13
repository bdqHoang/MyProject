using AutoMapper;
using MediatR;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.User.Commands.Delete
{
    public record RemoveRangeUserCommand(IEnumerable<Guid> data) : IRequest<bool>;
    public class RemoveRangeUserCommandHandler(IUserRepository _userRepository) : IRequestHandler<RemoveRangeUserCommand, bool>
    {
        public async Task<bool> Handle(RemoveRangeUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.RemoveRangeUserAsync(request.data);
        }
    }
}
