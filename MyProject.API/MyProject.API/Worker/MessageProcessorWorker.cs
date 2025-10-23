
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MyProject.API.Hubs;
using MyProject.Application.Features.Message.Command.Create;
using MyProject.Application.Interface;

namespace MyProject.API.Worker
{
    public class MessageProcessorWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageProcessorWorker> _logger;
        private readonly string _workerId;
        private readonly int _workerDelay;

        public MessageProcessorWorker(IServiceProvider serviceProvider, ILogger<MessageProcessorWorker> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _workerId = Guid.NewGuid().ToString("N")[..8];
            _workerDelay = configuration.GetValue<int>("MessageQueue:WorkerDelay", 1000);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Message Processor Worker {WorkerId} started.", _workerId);
            await Task.Delay(Random.Shared.Next(100, 500));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessMessagesAsync(stoppingToken);
                    await Task.Delay(_workerDelay, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Application đang shutdown
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Worker [{WorkerId}] error", _workerId);
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task ProcessMessagesAsync(CancellationToken stoppingToken)
        {
            try
            {
                // create new scope foreach message processing
                using var scope = _serviceProvider.CreateScope();

                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var queueService = scope.ServiceProvider.GetRequiredService<IMessageQueueService>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();

                // get message from queue
                var queuedMessage = await queueService.ConsumeMessageAsync();
                if (queuedMessage == null) return;

                var startTime = DateTime.UtcNow;
                var command = new SendMessageCommand()
                {
                    ConversationId = queuedMessage.ConversationId,
                    Content = queuedMessage.Content,
                    ParrentId = queuedMessage.ParrentId,
                    ReciverId = queuedMessage.ReciverId,
                    SenderId = queuedMessage.SenderId,
                    Type = queuedMessage.Type
                };

                var result = await sender.Send(command);
                await hubContext.Clients
                    .Group($"conversation_{result.ConversationId}")
                    .SendAsync("ReceiveMessage", result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
