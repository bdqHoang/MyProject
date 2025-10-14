using MyProject.Application.Interface;
using StackExchange.Redis;

namespace MyProject.Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }


        public async Task BlackListTokenAsync(string token, TimeSpan expiration)
        {
            var key = $"blacklist:{token}";

            await _db.StringSetAsync(key, "revoked", expiration);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        public async Task<string> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return (value.HasValue ? value.ToString() : null)!;
        }

        public async Task<bool> IsExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task<bool> IsTokenBlackListAsync(string token)
        {
            var key = $"blacklist:{token}";
            return await _db.KeyExistsAsync(key);
        }

        public async Task RemoveTokenFromBlackList(string token)
        {
            var key = $"blacklist:{token}";
            await _db.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {
            await _db.StringSetAsync(key, value, expiration);
        }
    }
}
