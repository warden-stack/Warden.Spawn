using System;
using System.Collections.Generic;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfiguration : IConfiguration
    {
        string WardenName { get; }
        long? IterationsCount { get; }
        bool OverrideCustomIntervals { get; }
        TimeSpan Interval { get; }
        IEnumerable<IWatcherSpawnWithHooksConfiguration> Watchers { get; }
        IEnumerable<ISpawnIntegration> Integrations { get; }
        IEnumerable<IWardenHookSpawnConfiguration> Hooks { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> GlobalWatcherHooks { get; }
        IEnumerable<IWatcherHookSpawnConfiguration> AggregatedWatcherHooks { get; }
    }
}