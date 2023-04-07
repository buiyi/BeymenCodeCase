namespace Beymen.Configuration.BackOffice.Repositories
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
        public string RedisConnectionString { get; set; }
    }
}