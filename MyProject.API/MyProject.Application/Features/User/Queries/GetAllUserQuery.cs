using AutoMapper;
using MediatR;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Queries
{
    public record GetAllUserQuery() : IRequest<IEnumerable<UserDetailRes>>;
    public class GetAllUserQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetAllUserQuery, IEnumerable<UserDetailRes>>
    {
        public async Task<IEnumerable<UserDetailRes>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
                

            return _mapper.Map<IEnumerable<UserDetailRes>>(users);

        }
    }
}
