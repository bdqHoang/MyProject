using AutoMapper;
using MediatR;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Entities;

namespace MyProject.Application.Features.User.Commands.Update
{
    public record UpdateUserCommand(Guid Id, UpdateUserReq data) : IRequest<UserDetailRes>;
    public class UpdateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper
        ) : IRequestHandler<UpdateUserCommand, UserDetailRes>
    {
        public async Task<UserDetailRes> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userDetail = await _userRepository.GetUserByIdAsync(request.Id);
            if (userDetail == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            var user = new Users();
             _mapper.Map(user, userDetail);

            _mapper.Map(request.data, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<UserDetailRes>(user);
        }
    }
}
