using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Core;
using Warden.Integrations;
using Warden.Spawn.Hooks;
using Warden.Watchers;
using System.Reflection;

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
                integrations, wardenHooks, globalWatcherHooks, aggregatedWatcherHooks,
                configuration.IterationsCount, configuration.Interval, configuration.OverrideCustomIntervals);

            return new WardenSpawn(spawnConfiguration);
        }

        private IEnumerable<IIntegration> ConfigureIntegrations(IEnumerable<ISpawnIntegration> integrations)
        {
            foreach (var integration in integrations)
            {
                var @namespace = integration.Configuration.GetType().Namespace;
                var configuratorTypeName = integration.Configuration.GetType()
                    .Name.Replace("Configuration", "Configurator");
                var configurationType = Type.GetType($"{@namespace}.{configuratorTypeName},{@namespace}");
                if (configurationType == null)
                    continue;

                var configurator = Activator.CreateInstance(configurationType);
                var method = configurator.GetType()
                    .GetMethod("Configure");
                var integrationInstance =
                    method.Invoke(configurator, new object[] {integration.Configuration}) as IIntegration;

                yield return integrationInstance;
            }
        }

        //TODO: Refactor finding integration by type name.
        private IEnumerable<ISpawnIntegration> SetIntegrationInstances(
            IEnumerable<ISpawnIntegration> configuredIntegrations,
            IEnumerable<IIntegration> integrations)
        {
            foreach (var configuredIntegration in configuredIntegrations)
            {
                var integration = integrations.FirstOrDefault(x => x.GetType().Name.ToLowerInvariant()
                                      .Contains(configuredIntegration.Name.ToLowerInvariant())) ??
                                  EmptyIntegration.Instance;

                configuredIntegration.Integration = integration;

                yield return configuredIntegration;
            }
        }

        private IEnumerable<IWatcherWithHooks> ConfigureWatchers(
            IEnumerable<IWatcherSpawnWithHooksConfiguration> watchers,
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
                    .GetMethod("Configure");
                var watcherInstance = method.Invoke(configurator, new object[] {watcher.Configuration}) as IWatcher;
                var watcherHooks = ConfigureWatcherHooks(watcher.Hooks, integrations);

                yield return new WatcherWithHooks(watcherInstance, watcherHooks);
            }
        }

        private Action<WatcherHooksConfiguration.Builder> ConfigureWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onStart = new List<Expression<Action<IWatcherCheck>>>();
            var onStartAsync = new List<Expression<Func<IWatcherCheck, Task>>>();
            var onSuccess = new List<Expression<Action<IWardenCheckResult>>>();
            var onSuccessAsync = new List<Expression<Func<IWardenCheckResult, Task>>>();
            var onFirstSuccess = new List<Expression<Action<IWardenCheckResult>>>();
            var onFirstSuccessAsync = new List<Expression<Func<IWardenCheckResult, Task>>>();
            var onFailure = new List<Expression<Action<IWardenCheckResult>>>();
            var onFailureAsync = new List<Expression<Func<IWardenCheckResult, Task>>>();
            var onFirstFailure = new List<Expression<Action<IWardenCheckResult>>>();
            var onFirstFailureAsync = new List<Expression<Func<IWardenCheckResult, Task>>>();
            var onCompleted = new List<Expression<Action<IWardenCheckResult>>>();
            var onCompletedAsync = new List<Expression<Func<IWardenCheckResult, Task>>>();
            var onError = new List<Expression<Action<Exception>>>();
            var onErrorAsync = new List<Expression<Func<Exception, Task>>>();
            var onFirstError = new List<Expression<Action<Exception>>>();
            var onFirstErrorAsync = new List<Expression<Func<Exception, Task>>>();

            foreach (var config in watcherConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                        x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WatcherHookType.OnStart:
                        onStart.Add(resolver.WatcherHooksResolver.OnStart(config.Configuration));
                        break;
                    case WatcherHookType.OnStartAsync:
                        onStartAsync.Add(resolver.WatcherHooksResolver.OnStartAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnSuccess:
                        onSuccess.Add(resolver.WatcherHooksResolver.OnSuccess(config.Configuration));
                        break;
                    case WatcherHookType.OnSuccessAsync:
                        onSuccessAsync.Add(resolver.WatcherHooksResolver.OnSuccessAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstSuccess:
                        onFirstSuccess.Add(resolver.WatcherHooksResolver.OnFirstSuccess(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstSuccessAsync:
                        onFirstSuccessAsync.Add(resolver.WatcherHooksResolver.OnFirstSuccessAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFailure:
                        onFailure.Add(resolver.WatcherHooksResolver.OnFailure(config.Configuration));
                        break;
                    case WatcherHookType.OnFailureAsync:
                        onFailureAsync.Add(resolver.WatcherHooksResolver.OnFailureAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstFailure:
                        onFirstFailure.Add(resolver.WatcherHooksResolver.OnFirstFailure(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstFailureAsync:
                        onFirstFailureAsync.Add(resolver.WatcherHooksResolver.OnFirstFailureAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnCompleted:
                        onCompleted.Add(resolver.WatcherHooksResolver.OnCompleted(config.Configuration));
                        break;
                    case WatcherHookType.OnCompletedAsync:
                        onCompletedAsync.Add(resolver.WatcherHooksResolver.OnCompletedAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnError:
                        onError.Add(resolver.WatcherHooksResolver.OnError(config.Configuration));
                        break;
                    case WatcherHookType.OnErrorAsync:
                        onErrorAsync.Add(resolver.WatcherHooksResolver.OnErrorAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstError:
                        onFirstError.Add(resolver.WatcherHooksResolver.OnFirstError(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstErrorAsync:
                        onFirstErrorAsync.Add(resolver.WatcherHooksResolver.OnFirstErrorAsync(config.Configuration));
                        break;
                }
            }

            return x => x.OnStart(onStart.ToArray())
                .OnStartAsync(onStartAsync.ToArray())
                .OnSuccess(onSuccess.ToArray())
                .OnSuccessAsync(onSuccessAsync.ToArray())
                .OnFirstSuccess(onFirstSuccess.ToArray())
                .OnFirstSuccessAsync(onFirstSuccessAsync.ToArray())
                .OnFailure(onFailure.ToArray())
                .OnFailureAsync(onFailureAsync.ToArray())
                .OnFirstFailure(onFirstFailure.ToArray())
                .OnFirstFailureAsync(onFirstFailureAsync.ToArray())
                .OnCompleted(onCompleted.ToArray())
                .OnCompletedAsync(onCompletedAsync.ToArray())
                .OnError(onError.ToArray())
                .OnErrorAsync(onErrorAsync.ToArray())
                .OnFirstError(onFirstError.ToArray())
                .OnFirstErrorAsync(onFirstErrorAsync.ToArray());
        }

        private Action<WatcherHooksConfiguration.Builder> ConfigureGlobalWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations) => ConfigureWatcherHooks(watcherConfigurations, integrations);

        private Action<AggregatedWatcherHooksConfiguration.Builder> ConfigureAggregatedWatcherHooks(
            IEnumerable<IWatcherHookSpawnConfiguration> watcherConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onSuccess = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onSuccessAsync = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            var onFirstSuccess = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onFirstSuccessAsync = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            var onFailure = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onFailureAsync = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            var onFirstFailure = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onFirstFailureAsync = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            var onCompleted = new List<Expression<Action<IEnumerable<IWardenCheckResult>>>>();
            var onCompletedAsync = new List<Expression<Func<IEnumerable<IWardenCheckResult>, Task>>>();
            var onError = new List<Expression<Action<IEnumerable<Exception>>>>();
            var onErrorAsync = new List<Expression<Func<IEnumerable<Exception>, Task>>>();
            var onFirstError = new List<Expression<Action<IEnumerable<Exception>>>>();
            var onFirstErrorAsync = new List<Expression<Func<IEnumerable<Exception>, Task>>>();

            foreach (var config in watcherConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                        x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WatcherHookType.OnSuccess:
                        onSuccess.Add(resolver.AggregatedWatcherHooksResolver.OnSuccess(config.Configuration));
                        break;
                    case WatcherHookType.OnSuccessAsync:
                        onSuccessAsync.Add(resolver.AggregatedWatcherHooksResolver.OnSuccessAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstSuccess:
                        onFirstSuccess.Add(resolver.AggregatedWatcherHooksResolver.OnFirstSuccess(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstSuccessAsync:
                        onFirstSuccessAsync.Add(
                            resolver.AggregatedWatcherHooksResolver.OnFirstSuccessAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFailure:
                        onFailure.Add(resolver.AggregatedWatcherHooksResolver.OnFailure(config.Configuration));
                        break;
                    case WatcherHookType.OnFailureAsync:
                        onFailureAsync.Add(resolver.AggregatedWatcherHooksResolver.OnFailureAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstFailure:
                        onFirstFailure.Add(resolver.AggregatedWatcherHooksResolver.OnFirstFailure(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstFailureAsync:
                        onFirstFailureAsync.Add(
                            resolver.AggregatedWatcherHooksResolver.OnFirstFailureAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnCompleted:
                        onCompleted.Add(resolver.AggregatedWatcherHooksResolver.OnCompleted(config.Configuration));
                        break;
                    case WatcherHookType.OnCompletedAsync:
                        onCompletedAsync.Add(
                            resolver.AggregatedWatcherHooksResolver.OnCompletedAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnError:
                        onError.Add(resolver.AggregatedWatcherHooksResolver.OnError(config.Configuration));
                        break;
                    case WatcherHookType.OnErrorAsync:
                        onErrorAsync.Add(resolver.AggregatedWatcherHooksResolver.OnErrorAsync(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstError:
                        onFirstError.Add(resolver.AggregatedWatcherHooksResolver.OnFirstError(config.Configuration));
                        break;
                    case WatcherHookType.OnFirstErrorAsync:
                        onFirstErrorAsync.Add(
                            resolver.AggregatedWatcherHooksResolver.OnFirstErrorAsync(config.Configuration));
                        break;
                }
            }

            return x => x.OnSuccess(onSuccess.ToArray())
                .OnSuccessAsync(onSuccessAsync.ToArray())
                .OnFirstSuccess(onFirstSuccess.ToArray())
                .OnFirstSuccessAsync(onFirstSuccessAsync.ToArray())
                .OnFailure(onFailure.ToArray())
                .OnFailureAsync(onFailureAsync.ToArray())
                .OnFirstFailure(onFirstFailure.ToArray())
                .OnFirstFailureAsync(onFirstFailureAsync.ToArray())
                .OnCompleted(onCompleted.ToArray())
                .OnCompletedAsync(onCompletedAsync.ToArray())
                .OnError(onError.ToArray())
                .OnErrorAsync(onErrorAsync.ToArray())
                .OnFirstError(onFirstError.ToArray())
                .OnFirstErrorAsync(onFirstErrorAsync.ToArray());
        }

        private Action<WardenHooksConfiguration.Builder> ConfigureHooks(
            IEnumerable<IWardenHookSpawnConfiguration> wardenHookConfigurations,
            IEnumerable<ISpawnIntegration> integrations)
        {
            var onStart = new List<Expression<Action>>();
            var onStartAsync = new List<Expression<Func<Task>>>();
            var onPause = new List<Expression<Action>>();
            var onPauseAsync = new List<Expression<Func<Task>>>();
            var onStop = new List<Expression<Action>>();
            var onStopAsync = new List<Expression<Func<Task>>>();
            var onError = new List<Expression<Action<Exception>>>();
            var onErrorAsync = new List<Expression<Func<Exception, Task>>>();
            var onIterationStart = new List<Expression<Action<long>>>();
            var onIterationStartAsync = new List<Expression<Func<long, Task>>>();
            var onIterationCompleted = new List<Expression<Action<IWardenIteration>>>();
            var onIterationCompletedAsync = new List<Expression<Func<IWardenIteration, Task>>>();

            foreach (var config in wardenHookConfigurations)
            {
                var resolver = integrations.FirstOrDefault(x =>
                        x.Name.ToLowerInvariant() == config.Use.ToLowerInvariant());
                if (resolver == null)
                    continue;

                switch (config.Type)
                {
                    case WardenHookType.OnStart:
                        onStart.Add(resolver.WardenHooksResolver.OnStart(config.Configuration));
                        break;
                    case WardenHookType.OnStartAsync:
                        onStartAsync.Add(resolver.WardenHooksResolver.OnStartAsync(config.Configuration));
                        break;
                    case WardenHookType.OnPause:
                        onPause.Add(resolver.WardenHooksResolver.OnPause(config.Configuration));
                        break;
                    case WardenHookType.OnPauseAsync:
                        onPauseAsync.Add(resolver.WardenHooksResolver.OnPauseAsync(config.Configuration));
                        break;
                    case WardenHookType.OnStop:
                        onStop.Add(resolver.WardenHooksResolver.OnStop(config.Configuration));
                        break;
                    case WardenHookType.OnStopAsync:
                        onStopAsync.Add(resolver.WardenHooksResolver.OnStopAsync(config.Configuration));
                        break;
                    case WardenHookType.OnError:
                        onError.Add(resolver.WardenHooksResolver.OnError(config.Configuration));
                        break;
                    case WardenHookType.OnErrorAsync:
                        onErrorAsync.Add(resolver.WardenHooksResolver.OnErrorAsync(config.Configuration));
                        break;
                    case WardenHookType.OnIterationStart:
                        onIterationStart.Add(resolver.WardenHooksResolver.OnIterationStart(config.Configuration));
                        break;
                    case WardenHookType.OnIterationStartAsync:
                        onIterationStartAsync.Add(
                            resolver.WardenHooksResolver.OnIterationStartAsync(config.Configuration));
                        break;
                    case WardenHookType.OnIterationCompleted:
                        onIterationCompleted.Add(resolver.WardenHooksResolver.OnIterationCompleted(config.Configuration));
                        break;
                    case WardenHookType.OnIterationCompletedAsync:
                        onIterationCompletedAsync.Add(
                            resolver.WardenHooksResolver.OnIterationCompletedAsync(config.Configuration));
                        break;
                }
            }

            return x => x.OnStart(onStart.ToArray())
                .OnStartAsync(onStartAsync.ToArray())
                .OnPause(onPause.ToArray())
                .OnPauseAsync(onPauseAsync.ToArray())
                .OnStop(onStop.ToArray())
                .OnStopAsync(onStopAsync.ToArray())
                .OnError(onError.ToArray())
                .OnErrorAsync(onErrorAsync.ToArray())
                .OnIterationStart(onIterationStart.ToArray())
                .OnIterationStartAsync(onIterationStartAsync.ToArray())
                .OnIterationCompleted(onIterationCompleted.ToArray())
                .OnIterationCompletedAsync(onIterationCompletedAsync.ToArray());
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            public WardenSpawnConfigurator Build() => new WardenSpawnConfigurator();
        }
    }
}