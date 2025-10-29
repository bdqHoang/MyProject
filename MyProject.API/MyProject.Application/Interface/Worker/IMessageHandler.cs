using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface.Worker
{
    public interface IMessageHandler<T> where T : IQueueableMessage
    {
        Task HandleMessageAsync(T message, CancellationToken cancellationToken);
    }
}
