using Microsoft.Extensions.Logging;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Interface;
using MyProject.Core.Enum;
using StackExchange.Redis;

namespace MyProject.Infrastructure.Services
{
    public class RedisStreamQueueService : IMessageQueueService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisStreamQueueService> _logger;
        private const string STREAM_KEY = "chat:messages:stream";
        private const string CONSUMER_GROUP = "message-processors";
        private const string CONSUMER_NAME = "processor";

        public RedisStreamQueueService(IConnectionMultiplexer redis, ILogger<RedisStreamQueueService> logger)
        {
            _redis = redis;
            _logger = logger;
            Task.Run(async () => await InitializeStreamAsync());
        }

        private async Task InitializeStreamAsync()
        {
            try
            {
                var db = _redis.GetDatabase();
                // create consumer group if not exists
                try
                {
                    await db.StreamCreateConsumerGroupAsync(    
                        STREAM_KEY,
                        CONSUMER_GROUP,
                        StreamPosition.NewMessages,
                        createStream: true
                        );
                    _logger.LogInformation("Created Redis Stream consumer group '{group}' successfully.", CONSUMER_GROUP);
                }
                catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
                {
                    _logger.LogInformation(ex, $"Consumer group '{CONSUMER_GROUP}' already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing Redis Stream");
            }
        }
        public async Task AcknowledgeMessageAsync(string messageId)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.StreamAcknowledgeAsync(STREAM_KEY, CONSUMER_GROUP, messageId);

                await db.StreamDeleteAsync(STREAM_KEY, [(RedisValue)messageId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error acknowledging message {messageId} in Redis Stream");
            }
        }

        public async Task<QueuedMessageDto?> ConsumeMessageAsync()
        {
            try
            {
                var db = _redis.GetDatabase();
                var messages = await db.StreamReadGroupAsync(
                    STREAM_KEY,
                    CONSUMER_GROUP,
                    CONSUMER_NAME,
                    ">",
                    count: 1,
                    noAck: false);

                if (messages == null || messages.Length == 0) return null;

                var message = messages[0];
                var values = message.Values.ToDictionary(
                    x => x.Name.ToString(),
                    x => x.Value.ToString());

                return new QueuedMessageDto()
                {
                    ConversationId = string.IsNullOrEmpty(values["ConversationId"]) ? null : Guid.Parse(values["ConversationId"]),
                    SenderId = Guid.Parse(values["SenderId"]),
                    ReciverId = Guid.Parse(values["ReciverId"]),
                    Content = values["Content"],
                    Type = (MessageType)int.Parse(values["Type"]),
                    QueueAt = DateTime.Parse(values["QueueAt"]),
                    Status = (MessageStatus)int.Parse(values["Status"]),
                    RetryCount = int.Parse(values["RetryCount"])
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Redis Stream");
                return null;
            }
        }

        public async Task RequeueMessageAsync(string messageId, string error)
        {
            try
            {
                var db = _redis.GetDatabase();
                var pendingMessages = await db.StreamPendingMessagesAsync(
                    STREAM_KEY,
                    CONSUMER_GROUP,
                    count: 1,
                    consumerName: CONSUMER_NAME);

                if (pendingMessages != null && pendingMessages.Length > 0)
                {
                    var pending = pendingMessages[0];

                    // if retry count < 3, just leaver it pending for reprocessing
                    // if retry count >= 3, remove from stream
                    if (pending.DeliveryCount >= 3)
                    {
                        var message = await db.StreamReadAsync(STREAM_KEY, messageId);

                        if (message.Length > 0)
                        {
                            var dlqKey = "chat:messages:dlq";
                            await db.StreamAddAsync(dlqKey, message[0].Values);

                            await db.StreamAcknowledgeAsync(STREAM_KEY, CONSUMER_GROUP, messageId);
                            await db.StreamDeleteAsync(STREAM_KEY, [(RedisValue)messageId]);
                        }
                        else
                        {
                            _logger.LogWarning($"No message with Id {messageId} found in Redis Stream for DLQ");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning($"No pending message with Id {messageId} found in Redis Stream");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error requeuing message {messageId} in Redis Stream");
            }
        }

        public async Task PublishMessageAsync(QueuedMessageDto message)
        {
            try
            {
                var db = _redis.GetDatabase();
                var fields = new NameValueEntry[] {
                    new("ConversationId",message.ConversationId?.ToString() ?? string.Empty),
                    new("SenderId",message.SenderId.ToString()),
                    new("ReciverId",message.ReciverId.ToString()),
                    new("ParrentId", message.ParrentId?.ToString() ?? string.Empty),
                    new("Content",message.Content),
                    new("Type",((int)message.Type).ToString()),
                    new("QueueAt",message.QueueAt.ToString("o")),
                    new("Status",((int)message.Status).ToString()),
                    new("RetryCount",message.RetryCount)
                };

                var messageId = await db.StreamAddAsync(STREAM_KEY, fields);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to Redis Stream");
            }
        }
    }
}
