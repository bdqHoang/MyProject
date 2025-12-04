using MyProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public string Data { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreateAt { get; set; }

        // navigator properties
        public Users User { get; set; } = null!;
    }
}
