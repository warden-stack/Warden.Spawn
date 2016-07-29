using System.Collections.Generic;

namespace Warden.Spawn.Core
{
    public interface IWardenSpawnConfiguration : IConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherSpawnConfiguration> Watchers { get; }
        IEnumerable<IIntegrationSpawnConfiguration> Integrations { get; }
    }
}