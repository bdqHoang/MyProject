using MyProject.Core.Entities;
using MyProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Message.DTO
{
    public class ConversationRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? LastMessage { get; set; }
        public ConversationType Type { get; set; } = ConversationType.Personal;
        public DateTime? LastMessageAt { get; set; }
        public int UnReadCount { get; set; }
        public List<ConversationParticipants> Participants { get; set; } = new List<ConversationParticipants>();
    }
}
