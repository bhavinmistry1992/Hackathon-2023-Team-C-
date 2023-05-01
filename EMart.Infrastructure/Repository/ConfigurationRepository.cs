using EMart.Core.ObjectModel;
using EMart.Core.Repository;

namespace EMart.Infrastructure.Repository
{
    public class ConfigurationRepository : Repository<Configurations>, IConfigurationRepository
    {
        public ConfigurationRepository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
