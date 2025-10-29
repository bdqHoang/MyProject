using MyProject.Application.Interface;
using MyProject.Application.Interface.Worker;

namespace MyProject.API.Worker
{
    public class MessageQueueProcessorWorker<T> : BackgroundService where T: class, IQueueableMessage
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageQueueProcessorWorker<T>> _logger;
        private readonly string _workerId;
        private readonly int _workerDelay;

        public MessageQueueProcessorWorker(IServiceProvider serviceProvider, ILogger<MessageQueueProcessorWorker<T>> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _workerId = Guid.NewGuid().ToString("N")[..8];
            _workerDelay = configuration.GetValue<int>("MessageQueue:WorkerDelay", 1000);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Message Processor Worker {WorkerId} started.", _workerId);
            //await Task.Delay(Random.Shared.Next(100, 500));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var processed = await ProcessMessagesAsync(stoppingToken);

                    if (!processed)
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

        private async Task<bool> ProcessMessagesAsync(CancellationToken stoppingToken)
        {
            // create new scope foreach message processing
            using var scope = _serviceProvider.CreateScope();

            var queueService = scope.ServiceProvider.GetRequiredService<IMessageQueueService<T>>();
            var queuedMessage = await queueService.ConsumeMessageAsync();
            // get message from queue
            if (queuedMessage == null) return false;

            try
            {
                _logger.LogInformation("🔄 Worker [{WorkerId}] processing message {MessageId}", _workerId, queuedMessage.StreamMessageId);
                var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();
                await handler.HandleMessageAsync(queuedMessage, stoppingToken);

                var startTime = DateTime.UtcNow;
                // acknowledge message
                await queueService.AcknowledgeMessageAsync(queuedMessage.StreamMessageId!);

                _logger.LogInformation("✅ Worker [{WorkerId}] processed message {MessageId} in {ElapsedMilliseconds} ms", 
                    _workerId, queuedMessage.StreamMessageId, (DateTime.UtcNow - startTime).TotalMilliseconds);
                return true;
            }
            catch (Exception ex)
            {
                await queueService.RequeueMessageAsync(
                    queuedMessage!.StreamMessageId ?? string.Empty,
                    ex.Message);
                _logger.LogWarning(ex, "❌ Worker [{WorkerId}] failed to process message {MessageId}", _workerId, queuedMessage?.StreamMessageId);
                return true;
            }

        }
    }
}
