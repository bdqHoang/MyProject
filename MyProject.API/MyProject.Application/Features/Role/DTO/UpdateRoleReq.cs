using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Role.DTO
{
    public class UpdateRoleReq
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
