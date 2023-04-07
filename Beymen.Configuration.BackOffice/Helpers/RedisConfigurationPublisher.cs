using Beymen.Configuration.BackOffice.Repositories;
using StackExchange.Redis;

namespace Beymen.Configuration.BackOffice.Helpers
{

    public class RedisConfigurationPublisher
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisConfigurationPublisher(IDatabaseSettings settings)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(settings.RedisConnectionString);
        }

        public void PublishConfigurationChange(string key)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            redisDb.Publish("ConfigurationChanged", key);
        }
    }
}
