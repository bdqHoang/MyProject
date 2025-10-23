using MyProject.Core.Enum;

namespace MyProject.Core.Entities
{
    public class QueueMessage
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SederId { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; }
        public DateTime QueueAt { get; set; }
        public MessageStatus Status { get; set; }
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
