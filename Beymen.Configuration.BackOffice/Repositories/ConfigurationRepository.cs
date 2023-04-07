using Beymen.Configuration.BackOffice.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Beymen.Configuration.BackOffice.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IMongoCollection<ConfigurationModel> _collection;

        public ConfigurationRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<ConfigurationModel>(settings.CollectionName);
        }

        public void Add(ConfigurationModel configuration)
        {
            configuration.Id = GetNextId();
            _collection.InsertOne(configuration);
        }

        public IEnumerable<ConfigurationModel> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public ConfigurationModel GetById(int id)
        {
            return _collection.Find(configuration => configuration.Id == id).FirstOrDefault();
        }

        public void Remove(int id)
        {
            _collection.DeleteOne(configuration => configuration.Id == id);
        }

        public void Update(ConfigurationModel configuration)
        {
            _collection.ReplaceOne(c => c.Id == configuration.Id, configuration);
        }
        private int GetNextId()
        {
            var maxId = _collection.Find(_ => true).SortByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
            return maxId + 1;
        }
    }
}
