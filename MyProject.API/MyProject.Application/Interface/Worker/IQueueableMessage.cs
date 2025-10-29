using System.Text.Json.Serialization;

namespace MyProject.Application.Interface.Worker
{
    public interface IQueueableMessage
    {
        [JsonIgnore]
        string? StreamMessageId { get; set; }
    }
}
