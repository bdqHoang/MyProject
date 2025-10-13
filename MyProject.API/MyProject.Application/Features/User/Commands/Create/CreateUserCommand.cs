using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.User.Commands.Create
{
    public record CreateUserCommand(CreateUserReq data) : IRequest<Guid>;

    public class AddUserCommandHandler(
        IUserRepository _userRepository,
        IPasswordHasher<Users> _passwordHasher,
        IMapper _mapper
        ) : IRequestHandler<CreateUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.data.Email);
            if (existingUser != null)
            {
                throw new ValidationException("Email already exists");
            }

            existingUser = await _userRepository.GetUserByEmailAsync(request.data.Phone);
            if (existingUser != null)
            {
                throw new ValidationException("Email already exists");
            }

            var user = _mapper.Map<Users>( request.data);
            user.Id = Guid.NewGuid();
            user.Password = _passwordHasher.HashPassword(user,request.data.Password);
            user.IsValidEmail = false;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.Status = true;

            var res = await _userRepository.AddUserAsync(user);

            return res;
        }
    }
}
