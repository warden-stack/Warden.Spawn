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
        public IEnumerable<ISpawnIntegration> Integrations { get; protected set; }
        public IEnumerable<IWardenHookSpawnConfiguration> Hooks { get; }
        public IEnumerable<IWatcherHookSpawnConfiguration> GlobalWatcherHooks { get; }
        public IEnumerable<IWatcherHookSpawnConfiguration> AggregatedWatcherHooks { get; }

        public WardenSpawnConfiguration(
            string wardenName,
            IEnumerable<IWatcherSpawnWithHooksConfiguration> watchers,
            IEnumerable<ISpawnIntegration> integrations,
            IEnumerable<IWardenHookSpawnConfiguration> hooks,
            IEnumerable<IWatcherHookSpawnConfiguration> globalWatcherHooks, 
            IEnumerable<IWatcherHookSpawnConfiguration> aggregatedWatcherHooks)
        {
            WardenName = wardenName;
            AggregatedWatcherHooks = aggregatedWatcherHooks;
            Watchers = watchers ?? Enumerable.Empty<IWatcherSpawnWithHooksConfiguration>();
            Integrations = integrations ?? Enumerable.Empty<ISpawnIntegration>();
            Hooks = hooks ?? Enumerable.Empty<IWardenHookSpawnConfiguration>();
            GlobalWatcherHooks = globalWatcherHooks ?? Enumerable.Empty<IWatcherHookSpawnConfiguration>();
            AggregatedWatcherHooks = aggregatedWatcherHooks ?? Enumerable.Empty<IWatcherHookSpawnConfiguration>();
        }
    }
}