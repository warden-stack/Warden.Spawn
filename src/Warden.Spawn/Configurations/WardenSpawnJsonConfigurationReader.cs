using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private readonly IEnumerable<IWatcherHookResolver> _watcherHookResolvers;
        private readonly IEnumerable<IExtension> _watchers;
        private readonly IEnumerable<IExtension> _integrations;
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

        protected WardenSpawnJsonConfigurationReader(IEnumerable<IExtension> watchers,
            IEnumerable<IExtension> integrations,
            IEnumerable<IWatcherHookResolver> watcherHookResolvers,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _watchers = watchers ?? Enumerable.Empty<IExtension>();
            _integrations = integrations ?? Enumerable.Empty<IExtension>();
            _watcherHookResolvers = watcherHookResolvers ?? Enumerable.Empty<IWatcherHookResolver>(); ;
            _jsonSerializerSettings = jsonSerializerSettings ?? _defaultJsonSerializerSettings;
        }

        public IWardenSpawnConfiguration Read(string configuration)
        {
            var spawnConfiguration = JsonConvert.DeserializeObject<dynamic>(configuration, _jsonSerializerSettings);
            var watcherConfigurations = ResolveWatcherConfigurations(spawnConfiguration.watchers);
            var integrationConfigurations = ResolveIntegrationConfigurations(spawnConfiguration.integrations);
            var wardenName = spawnConfiguration.wardenName.ToString();

            return new WardenSpawnConfiguration(wardenName, watcherConfigurations,
                integrationConfigurations, _watcherHookResolvers);
        }

        private IEnumerable<IWatcherSpawnWithHooksConfiguration> ResolveWatcherConfigurations(dynamic watchers)
        {
            if (watchers == null)
                yield break;

            foreach (var watcher in watchers)
            {
                var configuration = _watchers
                    .FirstOrDefault(x => x.Name.ToLowerInvariant().Equals(watcher.type.ToString().ToLowerInvariant()));
                if (configuration == null)
                    continue;

                var watcherConfigurationText = JsonConvert.SerializeObject(watcher.configuration);
                var watcherConfiguration = JsonConvert.DeserializeObject(watcherConfigurationText,
                    configuration.ConfigurationType) as IWatcherSpawnConfiguration;
                var hooksText = JsonConvert.SerializeObject(watcher.hooks);
                var hooksConfigurations =
                    JsonConvert.DeserializeObject<IEnumerable<WatcherHookSpawnConfiguration>>(hooksText)
                        as IEnumerable<WatcherHookSpawnConfiguration>;
                foreach (var hookConfig in hooksConfigurations)
                {
                    var resolver = _watcherHookResolvers.FirstOrDefault(x => x.Action.ToLowerInvariant().Equals(hookConfig.Action.ToLowerInvariant()));
                    if(resolver == null)
                        continue;

                    var cfg = JsonConvert.SerializeObject(hookConfig.Configuration);
                    hookConfig.Configuration = JsonConvert.DeserializeObject(cfg, resolver.ConfigurationType);
                }

                yield return new WatcherSpawnWithHooksConfiguration(watcherConfiguration, hooksConfigurations);
            }
        }

        private IEnumerable<IIntegrationSpawnConfiguration> ResolveIntegrationConfigurations(dynamic integrations)
        {
            if (integrations == null)
                yield break;

            foreach (var integration in integrations)
            {
                var configuration = _integrations.FirstOrDefault(x =>
                    x.Name.Equals(integration.type.ToString()));
                if (configuration == null)
                    continue;

                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText,
                    configuration.Type) as IIntegrationSpawnConfiguration;

                yield return integrationConfiguration;
            }
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            private readonly ISet<IWatcherHookResolver> _watcherHookResolvers = new HashSet<IWatcherHookResolver>();
            private readonly ISet<IExtension> _watchers = new HashSet<IExtension>();
            private readonly ISet<IExtension> _integrations = new HashSet<IExtension>();
            private JsonSerializerSettings _jsonSerializerSettings;

            public Builder WithWatcher(Func<IExtension> watcher)
            {
                if (watcher == null)
                {
                    throw new ArgumentNullException("Watcher can not be null.",
                        nameof(watcher));
                }

                _watchers.Add(watcher());

                return this;
            }

            public Builder WithIntegration(Func<IExtension> integration)
            {
                if (integration == null)
                {
                    throw new ArgumentNullException("Integration can not be null.",
                        nameof(integration));
                }

                _integrations.Add(integration());

                return this;
            }

            public Builder WithWatcherHooksResolver(Func<IWatcherHookResolver> watcherHookResolver)
            {
                if (watcherHookResolver == null)
                {
                    throw new ArgumentNullException("Watcher hooks resolver can not be null.",
                        nameof(watcherHookResolver));
                }

                _watcherHookResolvers.Add(watcherHookResolver());

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
                => new WardenSpawnJsonConfigurationReader(_watchers, _integrations,
                    _watcherHookResolvers, _jsonSerializerSettings);
        }
    }
}