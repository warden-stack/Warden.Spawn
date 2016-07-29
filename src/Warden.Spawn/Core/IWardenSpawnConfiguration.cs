using System.Collections.Generic;

namespace Warden.Spawn.Core
{
    public interface IWardenSpawnConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherConfiguration> Watchers { get; }
        IEnumerable<IIntegrationConfiguration> Integrations { get; }
    }
}