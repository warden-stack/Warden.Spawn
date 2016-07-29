using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Warden.Spawn.Core
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private readonly IDictionary<string, Type> _watcherConfigurationTypes;
        private readonly IDictionary<string, Type> _integrationConfigurationTypes;
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

        public WardenSpawnJsonConfigurationReader(IDictionary<string, Type> watcherConfigurationTypes,
            IDictionary<string, Type> integrationConfigurationTypes,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _watcherConfigurationTypes = watcherConfigurationTypes;
            _integrationConfigurationTypes = integrationConfigurationTypes;
            _jsonSerializerSettings = jsonSerializerSettings ?? _defaultJsonSerializerSettings;
        }

        public IWardenSpawnConfiguration Read(string configuration)
        {
            var spawnConfiguration = JsonConvert.DeserializeObject<dynamic>(configuration, _jsonSerializerSettings);
            var watcherConfigurations = ResolveWatcherConfigurations(spawnConfiguration);
            var integrationConfigurations = ResolveIntegrationConfigurations(spawnConfiguration);
            var wardenName = spawnConfiguration.wardenName.ToString();

            return new WardenSpawnConfiguration(wardenName, watcherConfigurations, integrationConfigurations);
        }

        private IEnumerable<IWatcherSpawnConfiguration> ResolveWatcherConfigurations(dynamic spawnConfiguration)
        {
            var watchers = spawnConfiguration.watchers;
            if (watchers == null)
                yield break;

            foreach (var watcher in watchers)
            {
                var watcherType = _watcherConfigurationTypes[watcher.type.ToString()];
                var watcherConfigurationText = JsonConvert.SerializeObject(watcher.configuration);
                var watcherConfiguration = JsonConvert.DeserializeObject(watcherConfigurationText, watcherType);

                yield return watcherConfiguration;
            }
        }

        private IEnumerable<IIntegrationSpawnConfiguration> ResolveIntegrationConfigurations(dynamic spawnConfiguration)
        {
            var integrations = spawnConfiguration.integrations;
            if (integrations == null)
                yield break;

            foreach (var integration in integrations)
            {
                var integrationType = _integrationConfigurationTypes[integration.type.ToString()];
                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText, integrationType);

                yield return integrationConfiguration;
            }
        }
    }
}