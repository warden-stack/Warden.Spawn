using System.Collections.Generic;
using System.Linq;
using Warden.Integrations;

namespace Warden.Spawn.Configurations
{
    class WardenSpawnConfigurationResolved : IWardenSpawnConfigurationResolved
    {
        public string Name => "Warden";
        public string WardenName { get; protected set; }
        public IEnumerable<IWatcherWithHooks> Watchers { get; protected set; }
        public IEnumerable<IIntegration> Integrations { get; protected set; }

        public WardenSpawnConfigurationResolved(
            string wardenName,
            IEnumerable<IWatcherWithHooks> watchers,
            IEnumerable<IIntegration> integrations)
        {
            WardenName = wardenName;
            Watchers = watchers ?? Enumerable.Empty<IWatcherWithHooks>();
            Integrations = integrations ?? Enumerable.Empty<IIntegration>();
        }
    }
}