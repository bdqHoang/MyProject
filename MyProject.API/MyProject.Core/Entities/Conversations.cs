using MyProject.Core.Enum;

namespace MyProject.Core.Entities
{
    public class Conversations
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? LastMessage { get; set; }
        public ConversationType Type { get; set; } = ConversationType.Personal;
        public DateTime? LastMessageAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }

        // Navigation properties
        public virtual ICollection<Messages> Messages { get; set; } = new List<Messages>();
        public virtual ICollection<ConversationParticipants> Participants { get; set; } = new List<ConversationParticipants>();
    }
}
