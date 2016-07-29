using Warden.Integrations;

namespace Warden.Spawn.Core
{
    public interface IIntegrationSpawnConfigurator<in T> : IConfigurator<T, IIntegration> where T : IIntegrationSpawnConfiguration
    {
    }
}