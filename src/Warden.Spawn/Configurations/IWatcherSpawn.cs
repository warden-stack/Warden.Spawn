using System;
using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawn
    {
        string Name { get; }
        IWatcherSpawnConfiguration Configuration { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; }
        Type ConfiguratorType { get; }

        void Configure(string name,
            IWatcherSpawnConfiguration configuration,
            IEnumerable<IWatcherHookSpawnConfiguration> hooks);
    }
}