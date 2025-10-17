using AutoMapper;
using MediatR;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Queries
{
    public record GetUserByEmailQuery(string data) : IRequest<UserDetailRes>;
    public class GetUserByEmailQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetUserByEmailQuery, UserDetailRes>
    {
        public async Task<UserDetailRes> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var resutl = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.data);

            return _mapper.Map<UserDetailRes>(resutl);
        }
    }
}
