using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private readonly IEnumerable<IWatcherHookActionResolver> _watcherHookResolvers;
        private readonly IEnumerable<IExtensionType> _watcherConfigurationTypes;
        private readonly IEnumerable<IExtensionType> _integrationConfigurationTypes;
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

        public WardenSpawnJsonConfigurationReader(IEnumerable<IExtensionType> watcherConfigurationTypes,
            IEnumerable<IExtensionType> integrationConfigurationTypes,
            IEnumerable<IWatcherHookActionResolver> watcherHookResolvers,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _watcherConfigurationTypes = watcherConfigurationTypes ?? Enumerable.Empty<IExtensionType>();
            _integrationConfigurationTypes = integrationConfigurationTypes ?? Enumerable.Empty<IExtensionType>();
            _watcherHookResolvers = watcherHookResolvers ?? Enumerable.Empty<IWatcherHookActionResolver>(); ;
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
                var configuration = _watcherConfigurationTypes
                    .FirstOrDefault(x => x.Name.Equals(watcher.type.ToString()));
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
                    var resolver = _watcherHookResolvers.FirstOrDefault(x => x.Action.ToLowerInvariant() == hookConfig.Action.ToLowerInvariant());
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
                var configuration = _integrationConfigurationTypes.FirstOrDefault(x =>
                    x.Name.Equals(integration.type.ToString()));
                if (configuration == null)
                    continue;

                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText,
                    configuration.Type) as IIntegrationSpawnConfiguration;

                yield return integrationConfiguration;
            }
        }
    }
}