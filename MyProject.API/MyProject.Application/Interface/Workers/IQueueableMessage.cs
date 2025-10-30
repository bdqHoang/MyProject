using System.Text.Json.Serialization;

namespace MyProject.Application.Interface.Workers
{
    public interface IQueueableMessage
    {
        [JsonIgnore]
        string? StreamMessageId { get; set; }
    }
}
