using EMart.Core.ObjectModel;
using EMart.Core.Repository;
using EMart.Core.Services;

namespace EMart.Infrastructure.Services
{
    public class ConfigurationsService : IConfigurationsService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationsService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public List<Configurations> GetAll()
        {
            return _configurationRepository.All().ToList();
        }
    }
}
