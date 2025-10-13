using AutoMapper;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Features.User.DTO;
using MyProject.Core.Entities;

namespace MyProject.Application.Common.Mappings
{
    public class UserProfile: Profile
    {
        public UserProfile() 
        {
            #region User create
            // map from DTO to Entity User Request
            CreateMap<CreateUserReq, Users>();
            CreateMap<RegisterReq, Users>();
            #endregion

            #region User detail
            // map from Entity to DTO User detail Response
            CreateMap<Users, UserDetailRes>();
            CreateMap<UserDetailRes,Users>();
            #endregion

            #region update user
            // map from DTO to Entity User Request
            CreateMap<UpdateUserReq, Users>();
            #endregion
        }
    }
}
