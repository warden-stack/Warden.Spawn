using Warden.Core;
using Warden.Spawn.Configurations;

namespace Warden.Spawn
{
    public class WardenSpawn : IWardenSpawn
    {
        private readonly IWardenSpawnConfigurationResolved _configuration;

        public WardenSpawn(IWardenSpawnConfigurationResolved configuration)
        {
            _configuration = configuration;
        }

        public IWarden Spawn()
        {
            var wardenConfiguration = WardenConfiguration
                .Create();

            foreach (var watcherWithHooks in _configuration.Watchers)
            {
                wardenConfiguration.AddWatcher(watcherWithHooks.Watcher, watcherWithHooks.Hooks);
            }
            foreach (var integration in _configuration.Integrations)
            {
                wardenConfiguration.AddIntegration(integration);
            }

            return WardenInstance.Create(_configuration.WardenName, wardenConfiguration.Build());
        }
    }
}