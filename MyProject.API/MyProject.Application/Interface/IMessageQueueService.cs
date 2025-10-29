using MyProject.Application.Interface.Worker;

namespace MyProject.Application.Interface
{
    public interface IMessageQueueService<T> where T : class, IQueueableMessage
    {
        Task PublishMessageAsync(T message);
        Task<T?> ConsumeMessageAsync();
        Task AcknowledgeMessageAsync(string messageId);
        Task RequeueMessageAsync(string messageId, string error);
    }
}
