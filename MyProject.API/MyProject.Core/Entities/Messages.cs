using MyProject.Core.Enum;

namespace MyProject.Core.Entities
{
    public class Messages
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid? ParrentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }

        // Navigation properties
        public virtual Users Sender { get; set; } = null!;
        public virtual Conversations Conversation { get; set; } = null!;
        public virtual Messages? ParentMessage { get; set; }
    }
}
