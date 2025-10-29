using MemoryPack;
using Microsoft.Extensions.Logging;
using MyProject.Application.Interface;
using MyProject.Application.Interface.Worker;
using StackExchange.Redis;

namespace MyProject.Infrastructure.Services
{
    public class RedisStreamQueueService<T> : IMessageQueueService<T> where T : class, IQueueableMessage
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisStreamQueueService<T>> _logger;
        private string _streamKey;
        private string _consumerGroup;
        private string _consumerName;

        public RedisStreamQueueService(IConnectionMultiplexer redis, ILogger<RedisStreamQueueService<T>> logger)
        {
            _redis = redis;
            _logger = logger;

            var typeName = typeof(T).Name.ToLower();
            _streamKey = $"stream:{typeName}";
            _consumerGroup = $"group:{typeName}";
            _consumerName = $"processor-{Guid.NewGuid():N}";

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
                        _streamKey,
                        _consumerGroup,
                        StreamPosition.NewMessages,
                        createStream: true
                        );
                    _logger.LogInformation("Created Redis Stream consumer group '{group}' successfully.", _consumerGroup);
                }
                catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
                {
                    _logger.LogInformation(ex, $"Consumer group '{_consumerGroup}' already exists");
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
                _logger.LogInformation($"Acknowledging message {messageId} in Redis Stream");
                var db = _redis.GetDatabase();
                await db.StreamAcknowledgeAsync(_streamKey, _consumerGroup, messageId);

                await db.StreamDeleteAsync(_streamKey, [(RedisValue)messageId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error acknowledging message {messageId} in Redis Stream");
            }
        }

        public async Task<T?> ConsumeMessageAsync()
        {
            try
            {
                var db = _redis.GetDatabase();
                var messages = await db.StreamReadGroupAsync(
                    _streamKey,
                    _consumerGroup,
                    _consumerName,
                    ">",
                    count: 1,
                    noAck: false);

                if (messages == null || messages.Length == 0) return null;

                var message = messages[0];
                var messageByte = message.Values.FirstOrDefault(x => x.Name == "Data").Value;
                
                if (messageByte.IsNullOrEmpty) return null;

                var bytes = (byte[])messageByte!;
                var result = MemoryPackSerializer.Deserialize<T>(bytes);
                result!.StreamMessageId = messages[0].Id;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Redis Stream");
                return null;
            }
        }

        public async Task RequeueMessageAsync(string messageId, string error)
        {
            if (messageId == null) throw new ArgumentNullException(nameof(messageId));
            try
            {
                var db = _redis.GetDatabase();

                // Lấy pending messages (ví dụ lấy tối đa 100 pending)
                var pendingMessages = await db.StreamPendingMessagesAsync(
                    _streamKey,
                    _consumerGroup,
                    count: 100,
                    consumerName: _consumerName // hoặc null để lấy tất cả
                );

                var messageToProcess = pendingMessages.FirstOrDefault(x => x.MessageId == messageId);

                if (messageToProcess.Equals(default(StreamPendingMessageInfo)))
                {
                    _logger.LogWarning($"No pending message with Id {messageId}");
                    return;
                }

                if (messageToProcess.DeliveryCount >= 3)
                {
                    var message = await db.StreamRangeAsync(_streamKey, messageId, messageId, count: 1);
                    if (message.Any())
                    {
                        var dlqKey = $"{_streamKey}:dlq";
                        await db.StreamAddAsync(dlqKey, message[0].Values);

                        await db.StreamAcknowledgeAsync(_streamKey, _consumerGroup, messageId);
                        await db.StreamDeleteAsync(_streamKey, new RedisValue[] { messageId });
                    }
                }
                else
                {
                    // Option: republish the message to retry (or claim it)
                    // Example: claim (transfer ownership) so this consumer can re-process:
                    // await db.StreamClaimAsync(_streamKey, _consumerGroup, CONSUMER_NAME, minIdleTimeInMilliseconds: 0, new RedisValue[] { messageId });

                    _logger.LogWarning($"Message {messageId} will be retried (deliveryCount={messageToProcess.DeliveryCount})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error requeuing message {messageId} in Redis Stream");
            }
        }

        public async Task PublishMessageAsync(T message)
        {
            try
            {
                _logger.LogInformation($"Publishing message to Redis Stream: {message}");
                var db = _redis.GetDatabase();

                byte[] messageByte = MemoryPackSerializer.Serialize(message);

                var fields = new NameValueEntry[] {
                    new("Data", messageByte)
                };

                var messageId = await db.StreamAddAsync(_streamKey, fields);
                _logger.LogInformation($"Published message to Redis Stream with Id: {messageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to Redis Stream");
                throw;
            }
        }
    }
}
