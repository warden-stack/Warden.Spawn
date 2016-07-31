using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfigurator : IWardenSpawnConfigurator
    {
        private readonly IEnumerable<IConfiguratorType> _watcherConfiguratorTypes;

        public WardenSpawnConfigurator(IEnumerable<IConfiguratorType> watcherConfiguratorTypes)
        {
            _watcherConfiguratorTypes = watcherConfiguratorTypes;
        }

        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var watchers = new List<IWatcher>();
            foreach (var watcherConfiguration in configuration.Watchers)
            {
                var extension = _watcherConfiguratorTypes.FirstOrDefault(x => x.Name.Equals(watcherConfiguration.Name));
                if (extension == null)
                    continue;

                var configurator = Activator.CreateInstance(extension.Type);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var watcher = method.Invoke(configurator, new object[] {watcherConfiguration}) as IWatcher;
                watchers.Add(watcher);
            }

            return new WardenSpawn(configuration.WardenName, watchers);
        }
    }
}