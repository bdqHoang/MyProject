using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.Auth.Command.Register
{
    public record RegisterCommand(RegisterReq data) : IRequest<bool>;
    public class RegisterCommandHandler(
        IUserRepository _userRepository,
        IRoleRepository _roleRepository,
        IPasswordHasher<Users> _passwordHasher,
        IMapper _mapper
        ) : IRequestHandler<RegisterCommand, bool>
    {
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Users>(request.data);
            user.Id = Guid.NewGuid();
            user.IsValidEmail = false;
            user.Password = _passwordHasher.HashPassword(user, request.data.Password);
            user.RoleId = (await _roleRepository.GetRoleByNameAsync(RoleName.User)).Id;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.Status = true;

            await _userRepository.AddUserAsync(user);

            return true;
        }
    }
}
