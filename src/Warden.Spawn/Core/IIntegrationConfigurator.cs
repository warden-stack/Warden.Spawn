using Warden.Integrations;

namespace Warden.Spawn.Core
{
    public interface IIntegrationConfigurator<in T> where T : IIntegrationConfiguration
    {
        IIntegration Configure(T configuration);
    }
}