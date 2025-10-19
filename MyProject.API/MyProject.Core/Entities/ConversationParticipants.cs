using MyProject.Core.Enum;

namespace MyProject.Core.Entities
{
    public class ConversationParticipants
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public ConversationRole Role { get; set; } = ConversationRole.Member;
        public bool IsMuted { get; set; } = false;
        public DateTime JoinedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }

        // Navigation properties
        public virtual Users User { get; set; } = null!;
        public virtual Conversations Conversation { get; set; } = null!;
    }
}
