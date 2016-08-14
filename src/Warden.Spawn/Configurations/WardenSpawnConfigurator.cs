using System;
using System.Collections;
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
        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var watchersWithHooks = new List<IWatcherWithHooks>();
            foreach (var watcher in configuration.Watchers)
            {
                var @namespace = watcher.Configuration.GetType().Namespace;
                var configuratorTypeName = watcher.Configuration.GetType().Name.Replace("Configuration", "Configurator");
                var configurationType = Type.GetType($"{@namespace}.{configuratorTypeName},{@namespace}");
                if(configurationType == null)
                    continue;

                var configurator = Activator.CreateInstance(configurationType);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var watcherInstance = method.Invoke(configurator, new object[] {watcher.Configuration}) as IWatcher;
                var hooks = ConfigureHooks(watcher.Hooks, configuration.Integrations);
                watchersWithHooks.Add(new WatcherWithHooks(watcherInstance, hooks));
            }
            var spawnConfiguration = new WardenSpawnConfigurationInstance(configuration.WardenName, watchersWithHooks, null);

            return new WardenSpawn(spawnConfiguration);
        }

        private Action<WatcherHooksConfiguration.Builder> ConfigureHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> hookSpawnConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onCompletedHooks = new List<Expression<Action<IWardenCheckResult>>>();
            var onCompletedAsyncHooks  = new List<Expression<Func<IWardenCheckResult, Task>>>();
            foreach (var config in hookSpawnConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WatcherHookType.OnCompleted:
                        var onCompleted = resolver.WatcherHooksResolver.OnCompleted(config.Configuration);
                        onCompletedHooks.Add(onCompleted);
                        break;
                    case WatcherHookType.OnCompletedAsync:
                        var onCompletedAsync = resolver.WatcherHooksResolver.OnCompletedAsync(config.Configuration);
                        onCompletedAsyncHooks.Add(onCompletedAsync);
                        break;
                }
            }

            Action<WatcherHooksConfiguration.Builder> hooks = x =>
                x.OnCompleted(onCompletedHooks.ToArray())
                 .OnCompletedAsync(onCompletedAsyncHooks.ToArray());

            return hooks;
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            public WardenSpawnConfigurator Build() => new WardenSpawnConfigurator();
        }
    }
}