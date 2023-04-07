namespace Beymen.Configuration.BackOffice.Repositories
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }

        public string RedisConnectionString { get; set; }

    }
}
