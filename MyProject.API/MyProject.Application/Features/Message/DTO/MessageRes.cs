using MyProject.Core.Enum;

namespace MyProject.Application.Features.Message.DTO
{
    public class MessageRes
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public Guid? ParrentId { get; set; }
        public string ParrentName { get; set; } = string.Empty;
        public string ParrentContent { get; set; } = string.Empty;
        public string Content { get; set; } = null!;
        public MessageType Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
