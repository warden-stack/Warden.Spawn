using System;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnFactory : IWardenSpawnFactory
    {
        private readonly IWardenSpawnFactoryConfiguration _configuration;

        protected WardenSpawnFactory(IWardenSpawnFactoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IWardenSpawn Resolve()
        {
            var spawnConfiguration = _configuration.ConfigurationReader.Read(_configuration.Configuration);
            var spawn = _configuration.Configurator.Configure(spawnConfiguration);

            return spawn;
        }


        public static Builder Create() => new Builder();

        public class Builder
        {
            private IWardenSpawnConfigurator _configurator;
            private IConfigurationReader<IWardenSpawnConfiguration> _configurationReader;
            private string _configuration;

            public Builder WithConfigurationReader(
                Func<IConfigurationReader<IWardenSpawnConfiguration>> configurationReader)
            {
                if (configurationReader == null)
                {
                    throw new ArgumentNullException("Configuration reader can not be null.",
                        nameof(configurationReader));
                }

                _configurationReader = configurationReader();

                return this;
            }

            public Builder WithConfigurator(Func<IWardenSpawnConfigurator> configurator)
            {
                if (configurator == null)
                {
                    throw new ArgumentNullException("Configurator can not be null.",
                        nameof(configurator));
                }

                _configurator = configurator();

                return this;
            }

            public Builder WithConfiguration(string configuration)
            {
                if (string.IsNullOrWhiteSpace(configuration))
                {
                    throw new ArgumentNullException("Configuration can not be empty.",
                        nameof(configuration));
                }

                _configuration = configuration;

                return this;
            }

            public WardenSpawnFactory Build()
                => new WardenSpawnFactory(new WardenSpawnFactoryConfiguration(
                    _configurationReader, _configurator, _configuration));
        }
    }
}