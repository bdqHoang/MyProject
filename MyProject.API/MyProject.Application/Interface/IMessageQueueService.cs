using MyProject.Application.Features.Message.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface
{
    public interface IMessageQueueService
    {
        Task PublishMessageAsync(QueuedMessageDto message);
        Task<QueuedMessageDto?> ConsumeMessageAsync();
        Task AcknowledgeMessageAsync(string messageId);
        Task RequeueMessageAsync(string messageId, string error);
    }
}
