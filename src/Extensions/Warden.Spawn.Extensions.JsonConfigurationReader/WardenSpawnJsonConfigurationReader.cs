using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;
using Warden.Spawn.Security;

namespace Warden.Spawn.Extensions.JsonConfigurationReader
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private ICredentialsManager _credentialsManager;
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

        protected WardenSpawnJsonConfigurationReader(ICredentialsManager credentialsManager,
            IEnumerable<Type> watchers,
            IEnumerable<Type> integrations,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _credentialsManager = credentialsManager;
            _watchers = watchers ?? Enumerable.Empty<Type>();
            _integrations = integrations ?? Enumerable.Empty<Type>();
            _jsonSerializerSettings = jsonSerializerSettings ?? _defaultJsonSerializerSettings;
        }

        public IWardenSpawnConfiguration Read(string configuration)
        {
            var spawnConfiguration = JsonConvert.DeserializeObject<dynamic>(configuration, _jsonSerializerSettings);
            var integrations = ResolveIntegrations(spawnConfiguration.integrations);
            var watchers = ResolveWatchers(spawnConfiguration.watchers, integrations);
            var wardenName = spawnConfiguration.wardenName.ToString();

            return new WardenSpawnConfiguration(wardenName, watchers, integrations);
        }

        private IEnumerable<IWatcherSpawnWithHooksConfiguration> ResolveWatchers(dynamic watchers,
            IEnumerable<ISpawnIntegration> integrations)
        {
            if (watchers == null)
                yield break;

            foreach (var watcher in watchers)
            {
                var configuration = _watchers.FirstOrDefault(x => x.Name.ToLowerInvariant()
                    .Replace("watcherspawn", string.Empty)
                    .Equals(watcher.type.ToString().ToLowerInvariant()));
                if (configuration == null)
                    continue;

                var watcherConfigurationText = JsonConvert.SerializeObject(watcher.configuration);
                var @namespace = configuration.Namespace;
                var configurationName = configuration.Name + "Configuration";
                var configurationType = Type.GetType($"{@namespace}.{configurationName},{@namespace}");
                var watcherConfiguration = JsonConvert.DeserializeObject(watcherConfigurationText,
                    configurationType) as IWatcherSpawnConfiguration;
                var hooksText = JsonConvert.SerializeObject(watcher.hooks);
                var hooksConfigurations =
                    JsonConvert.DeserializeObject<IEnumerable<WatcherHookSpawnConfiguration>>(hooksText)
                        as IEnumerable<WatcherHookSpawnConfiguration>;
                foreach (var hookConfig in hooksConfigurations)
                {
                    var integration = integrations.FirstOrDefault(x =>
                        x.Name.ToLowerInvariant().Equals(hookConfig.Use.ToLowerInvariant()));
                    if (integration == null)
                        continue;

                    var @integrationNamespace = integration.GetType().Namespace;
                    var watcherHooksConfigurationName = integration.GetType().Name + "WatcherHooksConfiguration";
                    var watcherHooksConfigurationType =
                        Type.GetType($"{@integrationNamespace}.{watcherHooksConfigurationName},{integrationNamespace}");
                    var cfg = JsonConvert.SerializeObject(hookConfig.Configuration);
                    hookConfig.Configuration = JsonConvert.DeserializeObject(cfg, watcherHooksConfigurationType);
                    _credentialsManager.SetConfiguration(hookConfig.Configuration);
                }

                yield return new WatcherSpawnWithHooksConfiguration(watcherConfiguration, hooksConfigurations);
            }
        }

        private IEnumerable<ISpawnIntegration> ResolveIntegrations(dynamic integrations)
        {
            if (integrations == null)
                yield break;

            foreach (var integration in integrations)
            {
                var configuration = _integrations.FirstOrDefault(x =>
                    x.Name.ToLowerInvariant().Replace("spawnintegration", string.Empty)
                        .Equals(integration.type.ToString().ToLowerInvariant()));
                if (configuration == null)
                    continue;

                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var @namespace = configuration.Namespace;
                var configurationName = configuration.Name + "Configuration";
                var configurationType = Type.GetType($"{@namespace}.{configurationName},{@namespace}");
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText,
                    configurationType) as ISpawnIntegrationConfiguration;
                _credentialsManager.SetConfiguration(integrationConfiguration);

                var integrationInstance = Activator.CreateInstance(configuration,
                    integrationConfiguration) as ISpawnIntegration;

                yield return integrationInstance;
            }
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            private ICredentialsManager _credentialsManager;
            private readonly IList<Type> _watchers = new List<Type>();
            private readonly IList<Type> _integrations = new List<Type>();
            private JsonSerializerSettings _jsonSerializerSettings;

            public Builder WithCredentialsManager(Func<ICredentialsManager> credentialsManager)
            {
                if (credentialsManager == null)
                {
                    throw new ArgumentNullException("Credentials manager can not be null.",
                        nameof(credentialsManager));
                }

                _credentialsManager = credentialsManager();

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
                => new WardenSpawnJsonConfigurationReader(_credentialsManager,
                    _watchers, _integrations, _jsonSerializerSettings);
        }
    }
}