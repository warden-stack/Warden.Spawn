using System;
using System.Collections.Generic;
using Warden.Core;
using Warden.Integrations;
using Warden.Spawn.Configurations.Logger;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfigurationInstance : IConfiguration
    {
        string WardenName { get; }
        long? IterationsCount { get; }
        bool OverrideCustomIntervals { get; }
        TimeSpan Interval { get; }
        WardenLoggerConfiguration Logger { get; }
        IEnumerable<IWatcherWithHooks> Watchers { get; }
        IEnumerable<IIntegration> Integrations { get; }
        Action<WardenHooksConfiguration.Builder> Hooks { get; }
        Action<WatcherHooksConfiguration.Builder> GlobalWatcherHooks { get; }
        Action<AggregatedWatcherHooksConfiguration.Builder> AggregatedWatcherHooks { get; set; }
    }
}