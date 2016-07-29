using System.Collections.Generic;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfiguration : IWardenSpawnConfiguration
    {
        public string WardenName { get; }
        public IEnumerable<IWatcherSpawnConfiguration> Watchers { get; }
        public IEnumerable<IIntegrationSpawnConfiguration> Integrations { get; }
    }
}