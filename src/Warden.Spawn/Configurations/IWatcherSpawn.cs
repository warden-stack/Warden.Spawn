using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawn
    {
        string Name { get; }
        string Group { get; }
        IWatcherSpawnConfiguration Configuration { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; }
    }
}