using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Auth.DTO
{
    public class LoginReq
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
