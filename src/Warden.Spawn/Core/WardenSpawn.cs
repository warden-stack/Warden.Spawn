using System.Collections.Generic;
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
            IEnumerable<IIntegration> integrations)
        {
            _wardenName = wardenName;
            _watchers = watchers;
            _integrations = integrations;
        }

        public IWarden Spawn()
        {
            var wardenConfiguration = WardenConfiguration
                .Create()
                .Build();

            return WardenInstance.Create(_wardenName, wardenConfiguration);
        }
    }
}