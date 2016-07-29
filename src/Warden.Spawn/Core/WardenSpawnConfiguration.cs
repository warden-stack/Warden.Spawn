using System.Collections.Generic;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfiguration : IWardenSpawnConfiguration
    {
        public string WardenName { get; }
        public IEnumerable<IWatcherConfiguration> Watchers { get; }
        public IEnumerable<IIntegrationConfiguration> Integrations { get; }
    }
}