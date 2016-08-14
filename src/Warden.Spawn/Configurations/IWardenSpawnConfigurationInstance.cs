using System.Collections.Generic;
using Warden.Integrations;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfigurationInstance : IConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherWithHooks> Watchers { get; }
        IEnumerable<IIntegration> Integrations { get; }
    }
}