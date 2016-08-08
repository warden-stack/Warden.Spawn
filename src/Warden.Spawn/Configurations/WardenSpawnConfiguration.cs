using System.Collections.Generic;
using System.Linq;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnConfiguration : IWardenSpawnConfiguration
    {
        public string Name => "Warden";
        public string WardenName { get; protected set; }
        public IEnumerable<IWatcherSpawnWithHooksConfiguration> Watchers { get; protected set; }
        public IEnumerable<IIntegrationSpawnConfiguration> Integrations { get; protected set; }
        public IEnumerable<IWatcherHookActionResolver> WatcherHookResolvers { get; protected set; }

        public WardenSpawnConfiguration(
            string wardenName,
            IEnumerable<IWatcherSpawnWithHooksConfiguration> watchers,
            IEnumerable<IIntegrationSpawnConfiguration> integrations, 
            IEnumerable<IWatcherHookActionResolver> watcherHookResolvers)
        {
            WardenName = wardenName;
            Watchers = watchers ?? Enumerable.Empty<IWatcherSpawnWithHooksConfiguration>();
            Integrations = integrations ?? Enumerable.Empty<IIntegrationSpawnConfiguration>();
            WatcherHookResolvers = watcherHookResolvers ?? Enumerable.Empty<IWatcherHookActionResolver>();
        }
    }
}