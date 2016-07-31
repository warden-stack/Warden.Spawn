using System;
using System.Collections.Generic;
using System.Linq;
using Warden.Core;
using Warden.Integrations;
using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public class WardenSpawn : IWardenSpawn
    {
        private readonly string _wardenName;
        private readonly IEnumerable<IWatcher> _watchers;
        private readonly IEnumerable<IIntegration> _integrations;

        public WardenSpawn(string wardenName, 
            IEnumerable<IWatcher> watchers, 
            IEnumerable<IIntegration> integrations = null)
        {
            _wardenName = wardenName;
            _watchers = watchers ?? Enumerable.Empty<IWatcher>();
            _integrations = integrations ?? Enumerable.Empty<IIntegration>();
        }

        public IWarden Spawn()
        {
            var wardenConfiguration = WardenConfiguration
                .Create();

            foreach (var watcher in _watchers)
            {
                wardenConfiguration.AddWatcher(watcher);
            }
            foreach (var integration in _integrations)
            {
                wardenConfiguration.AddIntegration(integration);
            }

            return WardenInstance.Create(_wardenName, wardenConfiguration.Build());
        }
    }
}