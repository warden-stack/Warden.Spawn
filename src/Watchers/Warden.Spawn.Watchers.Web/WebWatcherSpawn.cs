using System;
using System.Collections.Generic;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawn : IWatcherSpawn
    {
        public static IExtensionType Type => new ExtensionType(typeof(WebWatcherSpawn), typeof(WebWatcherSpawnConfiguration), "Web");
        public string Name { get; protected set; }
        public IWatcherSpawnConfiguration Configuration { get; protected set; }
        public IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; protected set; }
        public Type ConfiguratorType => typeof(WebWatcherSpawnConfigurator);
        public void Configure(string name, 
            IWatcherSpawnConfiguration configuration, 
            IEnumerable<IWatcherHookSpawnConfiguration> hooks)
        {
            Name = name;
            Configuration = configuration;
            Hooks = hooks;
        }
    }
}