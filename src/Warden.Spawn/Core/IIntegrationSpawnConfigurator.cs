using Warden.Integrations;

namespace Warden.Spawn.Core
{
    public interface IIntegrationSpawnConfigurator<in T> : IConfigurator where T : IIntegrationSpawnConfiguration
    {
        IIntegration Configure(T configuration);
    }
}