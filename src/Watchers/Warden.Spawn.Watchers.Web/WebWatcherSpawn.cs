using System.Collections.Generic;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawn : IWatcherSpawn
    {
        public static IExtension Extension
            => new Extension("Web", typeof(WebWatcherSpawn), typeof(WebWatcherSpawnConfiguration));

        public static IConfigurableTypes Type
            => new ConfigurableTypes(typeof(WebWatcherSpawnConfiguration),
                typeof(WebWatcherSpawnConfigurator));

        public string Name { get; protected set; }
        public IWatcherSpawnConfiguration Configuration { get; protected set; }
        public IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; protected set; }
    }
}