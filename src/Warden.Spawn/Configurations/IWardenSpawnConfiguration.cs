using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfiguration : IConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherSpawnWithHooksConfiguration> Watchers { get; }
        IEnumerable<ISpawnIntegration> Integrations { get; }
        IEnumerable<IWardenHookSpawnConfiguration> Hooks { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> GlobalWatcherHooks { get; }
    }
}