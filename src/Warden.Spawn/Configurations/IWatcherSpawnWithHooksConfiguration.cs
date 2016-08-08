using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawnWithHooksConfiguration
    {
        IWatcherSpawnConfiguration Configuration { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> Hooks { get; } 
    }
}