using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.User.Commands.Create
{
    public record CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid RoleId { get; set; }
        public string Avatar { get; set; } = null!;
    };

    public class AddUserCommandHandler(
        IUnitOfWork _unitOfWork,
        IPasswordHasher<Users> _passwordHasher,
        IMapper _mapper
        ) : IRequestHandler<CreateUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ValidationException("Email already exists");
            }

            existingUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Phone);
            if (existingUser != null)
            {
                throw new ValidationException("Email already exists");
            }

            var user = _mapper.Map<Users>(request);
            user.Id = Guid.NewGuid();
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            user.IsValidEmail = false;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.Status = true;

            await _unitOfWork.UserRepository.AddUserAsync(user);
            await _unitOfWork.CommitAsync();

            return user.Id;
        }
    }
}
