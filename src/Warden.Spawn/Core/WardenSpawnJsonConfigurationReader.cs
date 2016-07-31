using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Warden.Spawn.Core
{
    public class WardenSpawnJsonConfigurationReader : IConfigurationReader<IWardenSpawnConfiguration>
    {
        private readonly IEnumerable<IConfigurationType> _watcherConfigurationTypes;
        private readonly IEnumerable<IConfigurationType> _integrationConfigurationTypes;
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

        public WardenSpawnJsonConfigurationReader(IEnumerable<IConfigurationType> watcherConfigurationTypes,
            IEnumerable<IConfigurationType> integrationConfigurationTypes = null,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            _watcherConfigurationTypes = watcherConfigurationTypes ?? Enumerable.Empty<IConfigurationType>();
            _integrationConfigurationTypes = integrationConfigurationTypes ?? Enumerable.Empty<IConfigurationType>();
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
                var configuration = _watcherConfigurationTypes
                    .FirstOrDefault(x => x.Name.Equals(watcher.type.ToString()));
                if (configuration == null)
                    continue;

                var watcherConfigurationText = JsonConvert.SerializeObject(watcher.configuration);
                var watcherConfiguration = JsonConvert.DeserializeObject(watcherConfigurationText, configuration.Type);

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
                var configuration = _integrationConfigurationTypes.FirstOrDefault(x =>
                    x.Name.Equals(integration.type.ToString()));
                if (configuration == null)
                    continue;

                var integrationConfigurationText = JsonConvert.SerializeObject(integration.configuration);
                var integrationConfiguration = JsonConvert.DeserializeObject(integrationConfigurationText,
                    configuration.Type);

                yield return integrationConfiguration;
            }
        }
    }
}