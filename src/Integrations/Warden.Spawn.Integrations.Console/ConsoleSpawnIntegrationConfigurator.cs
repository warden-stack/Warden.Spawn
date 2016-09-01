using Warden.Integrations;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationConfigurator : IIntegrationSpawnConfigurator<ConsoleSpawnIntegrationConfiguration>
    {
        public IIntegration Configure(ConsoleSpawnIntegrationConfiguration configuration)
        {
            return EmptyIntegration.Instance;
        }
    }
}