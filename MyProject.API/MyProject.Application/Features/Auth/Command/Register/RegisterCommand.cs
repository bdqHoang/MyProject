using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using MyProject.Core.Enum;

namespace MyProject.Application.Features.Auth.Command.Register
{
    public record RegisterCommand : IRequest<bool>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Avatar { get; set; } = null!;
    };
    public class RegisterCommandHandler(
        IUnitOfWork _unitOfWork,
        IPasswordHasher<Users> _passwordHasher,
        IMapper _mapper
        ) : IRequestHandler<RegisterCommand, bool>
    {
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Users>(request);
            user.Id = Guid.NewGuid();
            user.IsValidEmail = false;
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            user.RoleId = (await _unitOfWork.RoleRepository.GetRoleByNameAsync(RoleName.User)).Id;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.Status = true;

            await _unitOfWork.UserRepository.AddUserAsync(user);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
