using RedisApp.Classes.Settings;
using RedisApp.Models;
using StackExchange.Redis;

namespace RedisApp.Classes
{
    public interface IRedisAdapter
    {
        Task<string> GetCacheAsync(string key);
        Task<bool> UpdateCacheAsyn(Cache request);
        Task<bool> DeleteCacheAsyn(string key);
    }

    public class RedisAdapter : IRedisAdapter
    {
        private readonly IRedisSettings redisSettings;
        private readonly IConnectionMultiplexer multiplexer;

        public RedisAdapter(IRedisSettings redisSettings)
        {
            // Create connection with Redis server, this could be in Startup class and then use dependency injection

            this.redisSettings = redisSettings;
            ConfigurationOptions configurationOptions = ConfigurationOptions.Parse($"{redisSettings.Host}:{redisSettings.Port}");
            this.multiplexer =ConnectionMultiplexer.Connect(configurationOptions);
        }

        public async Task<string> GetCacheAsync(string key)
        {
            string value = string.Empty;
            try
            {
                var redisDB = multiplexer.GetDatabase();
                value=await redisDB.StringGetAsync(key);
            }
            catch (RedisException ex)
            {
                Console.WriteLine("Cannot connect to redis service: ",ex.Message);
            }
            return value;
        }

        public async Task<bool> UpdateCacheAsyn(Cache request)
        {
            try
            {
                var redisDB = multiplexer.GetDatabase();// key could be a compound string ex:$"documentNumber:{request.Document},documentType:{type}"
                return await redisDB.StringSetAsync(request.Key,request.Value);// it is posible here save in value objects and define time for expire de data in memory
            }
            catch (RedisException ex)
            {
                Console.WriteLine("Cannot connect to redis service: ", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCacheAsyn(string key)
        {
            try
            {
                var redisDB = multiplexer.GetDatabase();
                return await redisDB.KeyDeleteAsync(key);
            }
            catch (RedisException ex)
            {
                Console.WriteLine("Cannot connect to redis service: ", ex.Message);
                return false;
            }
        }
    }
}
