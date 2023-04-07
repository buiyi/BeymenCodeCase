using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Beymen.ConfigurationLibrary;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;

//mongodb://127.0.0.1:27017/?directConnection=true
namespace ConfigurationLibrary
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<Configuration> _collection;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        private readonly Timer _timer;
        private readonly object _lockObject = new object(); // kilitleme nesnesi

        private Dictionary<string, object> _configurations = new Dictionary<string, object>();

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _client = new MongoClient(connectionString);
            var database = _client.GetDatabase("ConfigurationDatabase");
            _collection = database.GetCollection<Configuration>("Configurations");
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("redis:6379,abortConnect=false");
            
            ConsumeConfigurationChanges();

            // Timer'ı başlat
            _timer = new Timer(OnTimerElapsed, null, _refreshTimerIntervalInMs, _refreshTimerIntervalInMs);
        }

        public T GetValue<T>(string key)
        {
            lock (_lockObject) // Kilitleme ekleniyor
            {
                if (_configurations.TryGetValue(key, out var value))
                {
                    return (T)value;
                }

                // Değer henüz yüklenmemişse, veritabanından getir ve ekle
                var configuration = _collection.Find(x => x.ApplicationName == _applicationName && x.IsActive && x.Name == key)
                                               .SingleOrDefault();
                if (configuration == null)
                {
                    throw new KeyNotFoundException($"The key '{key}' was not found for application '{_applicationName}'");
                }

                value = (T)Convert.ChangeType(configuration.Value, typeof(T));
                _configurations.Add(key, value);

                return (T)value;
            }
        }

        //veritabanına ilk değerlerin eklenmesi için
        private void SeedData()
        {
            _collection.InsertMany(new List<Configuration>
            {
                new Configuration { Id = 1, Name = "SiteName", Type = "String", Value = "Boyner.com.tr", IsActive = true, ApplicationName = "SERVICE-A" },
                new Configuration { Id = 2, Name = "IsBasketEnabled", Type = "Boolean", Value = "True", IsActive = true, ApplicationName = "SERVICE-B" },
                new Configuration { Id = 3, Name = "MaxItemCount", Type = "Int", Value = "50", IsActive = false, ApplicationName = "SERVICE_A" }
            });

            // SeedData çağırıldığında, _configurations dictionary'sini temizle
            _configurations.Clear();
        }

        //verilen süre içinde yapılan değişikliklerin anlık olarak tüketilmesi için
        private void OnTimerElapsed(object state)
        {
            lock (_lockObject) // Kilitleme ekleniyor
            {
                var isClusterConnected = _client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;
                if (isClusterConnected)
                {
                    foreach (var configuration in _collection.Find(x => x.ApplicationName == _applicationName && x.IsActive).ToList())
                    {
                        if (!_configurations.TryGetValue(configuration.Name, out var currentValue) || !currentValue.Equals(configuration.Value))
                        {
                            _configurations[configuration.Name] = Convert.ChangeType(configuration.Value, Type.GetType($"System.{configuration.Type}"));
                        }
                    }
                }
            }
        }

        //back office tarafında yapılan değişikliklerin anlık olarak tüketilmesi için
        public void ConsumeConfigurationChanges()
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            var pubSub = _connectionMultiplexer.GetSubscriber();

            pubSub.Subscribe("ConfigurationChanged", (channel, key) =>
            {
                lock (_lockObject) // Kilitleme ekleniyor
                {
                    // Redis üzerinden gelen değişiklik olayını tüket
                    // Key değeri, değişen ConfigurationModel nesnesinin ismidir.
                    var configuration = _collection.Find(x => x.ApplicationName == _applicationName && x.IsActive && x.Name == key.ToString()).SingleOrDefault();
                    if (!_configurations.TryGetValue(configuration.Name, out var currentValue) || !currentValue.Equals(configuration.Value))
                    {
                        _configurations[configuration.Name] = Convert.ChangeType(configuration.Value, Type.GetType($"System.{configuration.Type}"));
                    }
                }
            });
        }
    }

}
