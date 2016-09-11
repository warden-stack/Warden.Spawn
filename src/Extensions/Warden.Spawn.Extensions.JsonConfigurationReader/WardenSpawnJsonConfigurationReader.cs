using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Warden.Spawn.Configurations;
using Warden.Spawn.Configurations.Logger;
using Warden.Spawn.Hooks;
using Warden.Spawn.Security;

namespace Warden.Spawn.Extensions.JsonConfigurationReader
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private readonly ICredentialsConfigurator _credentialsConfigurator;
        private readonly IEnumerable<Type> _watchers;
        private readonly IEnumerable<Type> _integrations;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private readonly JsonSerializerSettings _defaultJsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd H:mm:ss",
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Populate,
            NullValueHandling = NullValueHandling.Include,
            Converters = new List<JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter
                {
                    AllowIntegerValues = true,
                    CamelCaseText = true
                }
            }
        };

        protected WardenSpawnJsonConfigurationReader(ICredentialsConfigurator credentialsConfigurator,
            IEnumerable<Type> watchers,
            IEnumerable<Type> integrations,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _credentialsConfigurator = credentialsConfigurator;
            _watchers = watchers ?? Enumerable.Empty<Type>();
            _integrations = integrations ?? Enumerable.Empty<Type>();
            _jsonSerializerSettings = jsonSerializerSettings ?? _defaultJsonSerializerSettings;
        }

        public IWardenSpawnConfiguration Read(string configuration)
        {
            var basicConfiguration = JsonConvert.DeserializeObject<BasicWardenConfiguration>(configuration, _jsonSerializerSettings);
            var spawnConfiguration = JsonConvert.DeserializeObject<dynamic>(configuration, _jsonSerializerSettings);
            var wardenName = basicConfiguration.WardenName;
            var integrations = ResolveIntegrations(wardenName, spawnConfiguration.integrations);
            var watchers = ResolveWatchers(wardenName, spawnConfiguration.watchers, integrations);
            var hooks = ResolveHooks(wardenName, spawnConfiguration.hooks, integrations);
            var globalWatcherHooks = ResolveGlobalWatcherHooks(wardenName, spawnConfiguration.globalWatcherHooks,
                integrations);
            var aggregatedWatcherHooks = ResolveAggregatedWatcherHooks(wardenName, spawnConfiguration.aggregatedWatcherHooks,
                integrations);

            return new WardenSpawnConfiguration(wardenName, watchers, integrations, hooks,
                globalWatcherHooks, aggregatedWatcherHooks, basicConfiguration.IterationsCount,
                basicConfiguration.Interval, basicConfiguration.OverrideCustomIntervals,
                basicConfiguration.Logger);
        }

        private class BasicWardenConfiguration
        {
            public string WardenName { get; set; }
            public long? IterationsCount { get; set; }
            public bool OverrideCustomIntervals { get; set; }
            public TimeSpan Interval { get; set; }
            public WardenLoggerConfiguration Logger { get; set; }
        }

        private IEnumerable<IWatcherSpawnWithHooksConfiguration> ResolveWatchers(string wardenName,
            dynamic watchers, IEnumerable<ISpawnIntegration> integrations)
        {
            if (watchers == null)
                yield break;

            foreach (var watcher in watchers)
            {
                var watcherType = watcher.type.ToString().ToLowerInvariant();
                var watcherName = watcher.name.ToString();
                var watcherGroup = watcher.group.ToString();
                var configuration = _watchers.FirstOrDefault(x => x.Name.ToLowerInvariant()
                    .Replace("watcherspawn", string.Empty)
                    .Equals(watcherType));
                if (configuration == null)
                    continue;

                var watcherConfigurationText = JsonConvert.SerializeObject(watcher.configuration);
                var @namespace = configuration.Namespace;
                var configurationName = configuration.Name + "Configuration";
                var configurationType = Type.GetType($"{@namespace}.{configurationName},{@namespace}");
                var watcherConfiguration = JsonConvert.DeserializeObject(watcherConfigurationText,
                    configurationType) as IWatcherSpawnConfiguration;
                _credentialsConfigurator.SetConfiguration(wardenName, watcherConfiguration, watcherName);
                var hooksText = JsonConvert.SerializeObject(watcher.hooks);
                var hooksConfigurations = JsonConvert.DeserializeObject<IEnumerable<WatcherHookSpawnConfiguration>>(hooksText)
                        as IEnumerable<WatcherHookSpawnConfiguration>;
                foreach (var hookConfig in hooksConfigurations)
                {
                    SetWatcherHookConfiguration(wardenName, hookConfig, integrations, watcherType);
                }

                yield return new WatcherSpawnWithHooksConfiguration(watcherConfiguration, hooksConfigurations);
            }
        }

        private IEnumerable<IWatcherHookSpawnConfiguration> ResolveGlobalWatcherHooks(string wardenName,
            dynamic hooks, IEnumerable<ISpawnIntegration> integrations)
        {
            if (hooks == null)
                yield break;

            var hooksText = JsonConvert.SerializeObject(hooks);
            var hooksConfigurations = JsonConvert.DeserializeObject<IEnumerable<WatcherHookSpawnConfiguration>>(hooksText)
                    as IEnumerable<WatcherHookSpawnConfiguration>;

            foreach (var hookConfig in hooksConfigurations)
            {
                SetWatcherHookConfiguration(wardenName, hookConfig, integrations, type: "globalWatcherHooks");

                yield return hookConfig;
            }
        }

        private IEnumerable<IWatcherHookSpawnConfiguration> ResolveAggregatedWatcherHooks(string wardenName,
            dynamic hooks, IEnumerable<ISpawnIntegration> integrations)
        {
            if (hooks == null)
                yield break;

            var hooksText = JsonConvert.SerializeObject(hooks);
            var hooksConfigurations =
                JsonConvert.DeserializeObject<IEnumerable<WatcherHookSpawnConfiguration>>(hooksText)
                    as IEnumerable<WatcherHookSpawnConfiguration>;

            foreach (var hookConfig in hooksConfigurations)
            {
                SetWatcherHookConfiguration(wardenName, hookConfig, integrations, type: "aggregatedWatcherHooks");

                yield return hookConfig;
            }
        }

        private void SetWatcherHookConfiguration(string wardenName, 
            IWatcherHookSpawnConfiguration hookConfig, 
            IEnumerable<ISpawnIntegration> integrations, 
            string watcher = "",
            string type = "")
        {
            var integrationName = hookConfig.Use.ToLowerInvariant();
            var integration = integrations.FirstOrDefault(x =>
                x.Name.ToLowerInvariant().Equals(integrationName));
            if (integration == null)
                return;

            var integrationNamespace = integration.GetType().Namespace;
            var watcherHooksConfigurationName = integration.GetType().Name + "HooksConfiguration";
            var watcherHooksConfigurationType =
                Type.GetType($"{integrationNamespace}.{watcherHooksConfigurationName},{integrationNamespace}");
            var cfg = JsonConvert.SerializeObject(hookConfig.Configuration);
            var hookName = hookConfig.Type.ToString();
            hookConfig.Configuration = JsonConvert.DeserializeObject(cfg, watcherHooksConfigurationType);
            _credentialsConfigurator.SetConfiguration(wardenName, hookConfig.Configuration,
                integration: integrationName, watcher: watcher, hook: hookName, type: type); 
        }

        private IEnumerable<ISpawnIntegration> ResolveIntegrations(string wardenName, dynamic integrations)
        {
            if (integrations == null)
                yield break;

            foreach (var integration in integrations)
            {
                var integrationName = integration.type.ToString().ToLowerInvariant();
                var configuration = _integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant().Replace("spawnintegration", string.Empty)
                        .Equals(integrationName));
                if (configuration == null)
                    continue;

                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var @namespace = configuration.Namespace;
                var configurationName = configuration.Name + "Configuration";
                var configurationType = Type.GetType($"{@namespace}.{configurationName},{@namespace}");
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText,
                    configurationType) as ISpawnIntegrationConfiguration;
                _credentialsConfigurator.SetConfiguration(wardenName, integrationConfiguration, integration: integrationName);
                var integrationInstance = Activator.CreateInstance(configuration,
                    integrationConfiguration) as ISpawnIntegration;

                yield return integrationInstance;
            }
        }

        private IEnumerable<IWardenHookSpawnConfiguration> ResolveHooks(string wardenName,
            dynamic hooks, IEnumerable<ISpawnIntegration> integrations)
        {
            if (hooks == null)
                yield break;

            var hooksText = JsonConvert.SerializeObject(hooks);
            var hooksConfigurations = JsonConvert.DeserializeObject<IEnumerable<WardenHookSpawnConfiguration>>(hooksText)
                as IEnumerable<WardenHookSpawnConfiguration>;
            foreach (var hookConfig in hooksConfigurations)
            {
                var integrationName = hookConfig.Use.ToLowerInvariant();
                var integration = integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant().Equals(integrationName));
                if (integration == null)
                    continue;

                var integrationNamespace = integration.GetType().Namespace;
                var wardenHooksConfigurationName = integration.GetType().Name + "HooksConfiguration";
                var wardenHooksConfigurationType =
                    Type.GetType($"{integrationNamespace}.{wardenHooksConfigurationName},{integrationNamespace}");
                var cfg = JsonConvert.SerializeObject(hookConfig.Configuration);
                var hookName = hookConfig.Type.ToString();
                hookConfig.Configuration = JsonConvert.DeserializeObject(cfg, wardenHooksConfigurationType);
                _credentialsConfigurator.SetConfiguration(wardenName, hookConfig.Configuration,
                    integration: integrationName, hook: hookName, type: "wardenHooks");

                yield return hookConfig;
            }
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            private ICredentialsConfigurator _credentialsConfigurator;
            private readonly IList<Type> _watchers = new List<Type>();
            private readonly IList<Type> _integrations = new List<Type>();
            private JsonSerializerSettings _jsonSerializerSettings;

            public Builder WithCredentialsConfigurator(Func<ICredentialsConfigurator> credentialsConfigurator)
            {
                if (credentialsConfigurator == null)
                {
                    throw new ArgumentNullException("Credentials configurator can not be null.",
                        nameof(credentialsConfigurator));
                }

                _credentialsConfigurator = credentialsConfigurator();

                return this;
            }

            public Builder WithWatcher<T>()
            {
                _watchers.Add(typeof(T));

                return this;
            }

            public Builder WithIntegration<T>() where T : ISpawnIntegration
            {
                _integrations.Add(typeof(T));

                return this;
            }

            public Builder WithSerializerSettings(Func<JsonSerializerSettings> jsonSerializerSettings)
            {
                if (jsonSerializerSettings == null)
                {
                    throw new ArgumentNullException("JSON serializer settings can not be null.",
                        nameof(jsonSerializerSettings));
                }

                _jsonSerializerSettings = jsonSerializerSettings();

                return this;
            }

            public WardenSpawnJsonConfigurationReader Build()
                => new WardenSpawnJsonConfigurationReader(_credentialsConfigurator,
                    _watchers, _integrations, _jsonSerializerSettings);
        }
    }
}