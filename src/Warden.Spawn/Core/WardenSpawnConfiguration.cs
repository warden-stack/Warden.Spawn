using System.Collections.Generic;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfiguration : IWardenSpawnConfiguration
    {
        public string WardenName { get; protected set; }
        public IEnumerable<IWatcherSpawnConfiguration> Watchers { get; protected set; }
        public IEnumerable<IIntegrationSpawnConfiguration> Integrations { get; protected set; }

        protected WardenSpawnConfiguration()
        {
        }

        public WardenSpawnConfiguration(string wardenName,
            IEnumerable<IWatcherSpawnConfiguration> watchers,
            IEnumerable<IIntegrationSpawnConfiguration> integrations)
        {
            WardenName = wardenName;
            Watchers = watchers;
            Integrations = integrations;
        }
    }
}