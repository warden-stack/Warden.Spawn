using System;
using System.Collections.Generic;
using Warden.Core;
using Warden.Integrations;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnConfigurationInstance : IConfiguration
    {
        string WardenName { get; }
        IEnumerable<IWatcherWithHooks> Watchers { get; }
        IEnumerable<IIntegration> Integrations { get; }
        Action<WardenHooksConfiguration.Builder> Hooks { get; }
    }
}