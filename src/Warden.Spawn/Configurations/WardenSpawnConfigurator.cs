using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Warden.Core;
using Warden.Integrations;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnConfigurator : IWardenSpawnConfigurator
    {
        public IWardenSpawn Configure(IWardenSpawnConfiguration configuration)
        {
            var integrations = ConfigureIntegrations(configuration.Integrations);
            var configuredIntegrations = SetIntegrationInstances(configuration.Integrations, integrations);
            var watchers = ConfigureWatchers(configuration.Watchers, configuredIntegrations);
            var wardenHooks = ConfigureHooks(configuration.Hooks, configuredIntegrations);
            var globalWatcherHooks = ConfigureGlobalWatcherHooks(configuration.GlobalWatcherHooks,
                configuredIntegrations);
            var aggregatedWatcherHooks = ConfigureAggregatedWatcherHooks(configuration.AggregatedWatcherHooks,
                configuration.Integrations);
            var spawnConfiguration = new WardenSpawnConfigurationInstance(configuration.WardenName, watchers,
                integrations, wardenHooks, globalWatcherHooks, aggregatedWatcherHooks);

            return new WardenSpawn(spawnConfiguration);
        }

        private IEnumerable<IIntegration> ConfigureIntegrations(IEnumerable<ISpawnIntegration> integrations)
        {
            foreach (var integration in integrations)
            {
                var @namespace = integration.Configuration.GetType().Namespace;
                var configuratorTypeName = integration.Configuration.GetType().Name.Replace("Configuration", "Configurator");
                var configurationType = Type.GetType($"{@namespace}.{configuratorTypeName},{@namespace}");
                if (configurationType == null)
                    continue;

                var configurator = Activator.CreateInstance(configurationType);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var integrationInstance = method.Invoke(configurator, new object[] { integration.Configuration }) as IIntegration;

                yield return integrationInstance;
            }
        }

        //TODO: Refactor finding integration by type name.
        private IEnumerable<ISpawnIntegration> SetIntegrationInstances(IEnumerable<ISpawnIntegration> configuredIntegrations,
            IEnumerable<IIntegration> integrations)
        {
            foreach (var configuredIntegration in configuredIntegrations)
            {
                var integration = integrations.FirstOrDefault(x => x.GetType().Name.ToLowerInvariant()
                    .Contains(configuredIntegration.Name.ToLowerInvariant())) ?? EmptyIntegration.Instance;

                configuredIntegration.Integration = integration;

                yield return configuredIntegration;
            }
        }

        private IEnumerable<IWatcherWithHooks> ConfigureWatchers(IEnumerable<IWatcherSpawnWithHooksConfiguration> watchers, 
            IEnumerable<ISpawnIntegration> integrations)
        {
            foreach (var watcher in watchers)
            {
                var @namespace = watcher.Configuration.GetType().Namespace;
                var configuratorTypeName = watcher.Configuration.GetType().Name.Replace("Configuration", "Configurator");
                var configurationType = Type.GetType($"{@namespace}.{configuratorTypeName},{@namespace}");
                if (configurationType == null)
                    continue;

                var configurator = Activator.CreateInstance(configurationType);
                var method = configurator.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("Configure"));
                var watcherInstance = method.Invoke(configurator, new object[] { watcher.Configuration }) as IWatcher;
                var watcherHooks = ConfigureWatcherHooks(watcher.Hooks, integrations);

                yield return new WatcherWithHooks(watcherInstance, watcherHooks);
            }
        }

        private Action<WatcherHooksConfiguration.Builder> ConfigureWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onCompletedHooks = new List<Expression<Action<IWardenCheckResult>>>();
            var onCompletedAsyncHooks  = new List<Expression<Func<IWardenCheckResult, Task>>>();
            foreach (var config in watcherConfigurations)
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

        private Action<WatcherHooksConfiguration.Builder> ConfigureGlobalWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations) => ConfigureWatcherHooks(watcherConfigurations, integrations);

        private Action<AggregatedWatcherHooksConfiguration.Builder> ConfigureAggregatedWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onCompletedHooks = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onCompletedAsyncHooks = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            foreach (var config in watcherConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WatcherHookType.OnCompleted:
                        var onCompleted = resolver.AggregatedWatcherHooksResolver.OnCompleted(config.Configuration);
                        onCompletedHooks.Add(onCompleted);
                        break;
                    case WatcherHookType.OnCompletedAsync:
                        var onCompletedAsync = resolver.AggregatedWatcherHooksResolver.OnCompletedAsync(config.Configuration);
                        onCompletedAsyncHooks.Add(onCompletedAsync);
                        break;
                }
            }

            Action<AggregatedWatcherHooksConfiguration.Builder> hooks = x =>
                x.OnCompleted(onCompletedHooks.ToArray())
                 .OnCompletedAsync(onCompletedAsyncHooks.ToArray());

            return hooks;
        }

        private Action<WardenHooksConfiguration.Builder> ConfigureHooks(
            IEnumerable<IWardenHookSpawnConfiguration> wardenHookConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onIterationCompletedHooks = new List<Expression<Action<IWardenIteration>>>();
            var onIterationCompletedAsyncHooks = new List<Expression<Func<IWardenIteration, Task>>>();
            foreach (var config in wardenHookConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WardenHookType.OnIterationCompleted:
                        var onCompleted = resolver.WardenHooksResolver.OnIterationCompleted(config.Configuration);
                        onIterationCompletedHooks.Add(onCompleted);
                        break;
                    case WardenHookType.OnIterationCompletedAsync:
                        var onCompletedAsync =
                            resolver.WardenHooksResolver.OnIterationCompletedAsync(config.Configuration);
                        onIterationCompletedAsyncHooks.Add(onCompletedAsync);
                        break;
                }
            }

            Action<WardenHooksConfiguration.Builder> hooks = x =>
                x.OnIterationCompleted(onIterationCompletedHooks.ToArray())
                    .OnIterationCompletedAsync(onIterationCompletedAsyncHooks.ToArray());

            return hooks;
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            public WardenSpawnConfigurator Build() => new WardenSpawnConfigurator();
        }
    }
}