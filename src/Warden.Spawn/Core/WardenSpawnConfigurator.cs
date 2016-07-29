using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Warden.Integrations;
using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public class WardenSpawnConfigurator : IWardenSpawnConfigurator
    {
        private readonly IEnumerable<IExtension> _extensions;

        public WardenSpawnConfigurator(IEnumerable<IExtension> extensions)
        {
            _extensions = extensions;
        }

        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var watchers = new List<IWatcher>();
            var integrations = new List<IIntegration>();
            foreach (var watcherConfiguration in configuration.Watchers)
            {
                var extension = _extensions.Where(x => x.Type == ExtensionType.Watcher)
                    .FirstOrDefault(x => x.Name == watcherConfiguration.Name);
                if (extension == null)
                    continue;

                var configurator = Activator.CreateInstance(extension.ConfiguratorType);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var watcher = method.Invoke(configurator, new object[] {watcherConfiguration}) as IWatcher;
                watchers.Add(watcher);
            }

            return new WardenSpawn(configuration.WardenName, watchers, integrations);
        }
    }
}