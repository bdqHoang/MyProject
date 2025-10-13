using AutoMapper;
using MyProject.Application.Features.Role.DTO;
using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Common.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            #region create role
            // map from DTO to Entity Role Request
            CreateMap<CreateRoleReq, Roles>();
            #endregion

            #region update role
            // update role
            CreateMap<UpdateRoleReq, Roles>();
            #endregion

            #region role detail
            // map from Entity to DTO Role detail Response
            CreateMap<Roles, RoleDetailRes>();
            #endregion
        }
    }
}
