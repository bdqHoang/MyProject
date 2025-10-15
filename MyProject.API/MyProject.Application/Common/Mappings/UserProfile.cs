using AutoMapper;
using MyProject.Application.Features.Auth.Command.Register;
using MyProject.Application.Features.User.Commands.Create;
using MyProject.Application.Features.User.Commands.Update;
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
            CreateMap<CreateUserCommand, Users>();
            CreateMap<RegisterCommand, Users>();
            #endregion

            #region User detail
            // map from Entity to DTO User detail Response
            CreateMap<Users, UserDetailRes>();
            CreateMap<UserDetailRes,Users>();
            #endregion

            #region update user
            // map from DTO to Entity User Request
            CreateMap<UpdateUserCommand, Users>();
            #endregion
        }
    }
}
