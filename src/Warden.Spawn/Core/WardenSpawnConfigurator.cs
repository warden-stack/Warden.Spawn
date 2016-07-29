using System.Collections.Generic;
using Warden.Integrations;
using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfigurator : IWardenSpawnConfigurator
    {
        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var watchers = new List<IWatcher>();
            var integrations = new List<IIntegration>();

            return new WardenSpawn(configuration.WardenName, watchers, integrations);
        }
    }
}