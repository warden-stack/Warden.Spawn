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

        public WardenSpawnConfiguration(
            string wardenName,
            IEnumerable<IWatcherSpawnWithHooksConfiguration> watchers,
            IEnumerable<ISpawnIntegration> integrations,
            IEnumerable<IWardenHookSpawnConfiguration> globalWatcherHooks)
        {
            WardenName = wardenName;
            Watchers = watchers ?? Enumerable.Empty<IWatcherSpawnWithHooksConfiguration>();
            Integrations = integrations ?? Enumerable.Empty<ISpawnIntegration>();
            Hooks = globalWatcherHooks ?? Enumerable.Empty<IWardenHookSpawnConfiguration>();
        }
    }
}