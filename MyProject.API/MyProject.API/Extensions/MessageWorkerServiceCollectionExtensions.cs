using MyProject.API.Worker;
using MyProject.Application.Interface.Workers;

namespace MyProject.API.Extensions
{
    public static class MessageWorkerServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageWorker<T>(this IServiceCollection services) where T: class, IQueueableMessage
        {
            services.AddHostedService<MessageQueueProcessorWorker<T>>();
            return services;
        }
    }
}
