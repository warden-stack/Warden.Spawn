using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnConfigurator : IWardenSpawnConfigurator
    {
        private readonly IEnumerable<IConfigurableTypes> _configurableTypes;

        protected WardenSpawnConfigurator(IEnumerable<IConfigurableTypes> configurableTypes)
        {
            _configurableTypes = configurableTypes;
        }

        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var watchersWithHooks = new List<IWatcherWithHooks>();
            foreach (var watcher in configuration.Watchers)
            {
                var configuratorType = _configurableTypes
                    .FirstOrDefault(x => x.Configuration == watcher.Configuration.GetType())
                    ?.Configurator;
                if(configuratorType == null)
                    continue;

                var configurator = Activator.CreateInstance(configuratorType);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var watcherInstance = method.Invoke(configurator, new object[] {watcher.Configuration}) as IWatcher;
                var hooks = ConfigureHooks(watcher.Hooks, configuration.WatcherHookResolvers);
                watchersWithHooks.Add(new WatcherWithHooks(watcherInstance, hooks));
            }
            var spawnConfiguration = new WardenSpawnConfigurationResolved(configuration.WardenName, watchersWithHooks, null);

            return new WardenSpawn(spawnConfiguration);
        }

        private Action<WatcherHooksConfiguration.Builder> ConfigureHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> hookSpawnConfigurations,
            IEnumerable<IWatcherHookResolver> watcherHookResolvers)
        {
            Expression<Action<IWardenCheckResult>> onCompleted = null;
            Expression<Func<IWardenCheckResult, Task>> onCompletedAsync = null;
            foreach (var config in hookSpawnConfigurations)
            {
                var resolver = watcherHookResolvers.FirstOrDefault(x => x.Action.ToLowerInvariant() == config.Action.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WatcherHookType.OnCompleted:
                        onCompleted = resolver.Resolver.OnCompleted(config.Configuration);
                        break;
                    case WatcherHookType.OnCompletedAsync:
                        onCompletedAsync = resolver.Resolver.OnCompletedAsync(config.Configuration);
                        break;
                }
            }

            Action<WatcherHooksConfiguration.Builder> hooks = x =>
                x.OnCompleted(onCompleted)
                 .OnCompletedAsync(onCompletedAsync);

            return hooks;
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            private readonly ISet<IConfigurableTypes> _configurableTypes = new HashSet<IConfigurableTypes>();

            public Builder With(Func<IConfigurableTypes> configurableTypes)
            {
                if (configurableTypes == null)
                {
                    throw new ArgumentNullException("Configurable types can not be null.",
                        nameof(configurableTypes));
                }

                _configurableTypes.Add(configurableTypes());

                return this;
            }

            public WardenSpawnConfigurator Build() => new WardenSpawnConfigurator(_configurableTypes);
        }
    }
}