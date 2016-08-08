using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public class WatcherSpawnWithHooksConfiguration : IWatcherSpawnWithHooksConfiguration
    {
        public IWatcherSpawnConfiguration Configuration { get; }
        public IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; }

        public WatcherSpawnWithHooksConfiguration(IWatcherSpawnConfiguration configuration, 
            IEnumerable<IWatcherHookSpawnConfiguration> hooks)
        {
            Configuration = configuration;
            Hooks = hooks;
        }
    }
}