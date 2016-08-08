using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfiguration : IConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherSpawnWithHooksConfiguration> Watchers { get; }
        IEnumerable<IIntegrationSpawnConfiguration> Integrations { get; }
        IEnumerable<IWatcherHookActionResolver> WatcherHookResolvers { get; }
    }
}