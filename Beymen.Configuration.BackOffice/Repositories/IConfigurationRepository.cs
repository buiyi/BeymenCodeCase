using Beymen.Configuration.BackOffice.Models;

namespace Beymen.Configuration.BackOffice.Repositories
{
    public interface IConfigurationRepository
    {
        void Add(ConfigurationModel configuration);
        void Update(ConfigurationModel configuration);
        void Remove(int id);
        ConfigurationModel GetById(int id);
        IEnumerable<ConfigurationModel> GetAll();
    }
}