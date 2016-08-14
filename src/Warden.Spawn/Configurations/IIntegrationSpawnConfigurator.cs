using Warden.Integrations;

namespace Warden.Spawn.Configurations
{
    public interface IIntegrationSpawnConfigurator<in T> : IConfigurator<T, IIntegration> where T : ISpawnIntegrationConfiguration
    {
    }
}