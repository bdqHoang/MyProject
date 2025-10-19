using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Message.DTO
{
    public class ParticipantsRes
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
    }
}
