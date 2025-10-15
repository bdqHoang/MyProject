using AutoMapper;
using MediatR;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.User.Commands.Update
{
    public record UpdateUserCommand : IRequest<UserDetailRes>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid RoleId { get; set; }
        public string Avatar { get; set; } = null!;
    };
    public class UpdateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper
        ) : IRequestHandler<UpdateUserCommand, UserDetailRes>
    {
        public async Task<UserDetailRes> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existsUser = await _userRepository.GetUserByIdAsync(request.Id);
            if (existsUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _mapper.Map(request, existsUser);
            existsUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(existsUser);

            return _mapper.Map<UserDetailRes>(existsUser);
        }
    }
}
