using MyProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Message.DTO
{
    public class QueuedMessageDto
    {
        public Guid? ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        public Guid? ParrentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; }
        public DateTime QueueAt { get; set; }
        public MessageStatus Status { get; set; }
        public int RetryCount { get; set; }
    }
}
