using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawn
    {
        string Name { get; }
        IWatcherSpawnConfiguration Configuration { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; }
    }
}