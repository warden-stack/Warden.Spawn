namespace Warden.Spawn.Configurations
{
    public class WardenSpawnFactoryConfiguration : IWardenSpawnFactoryConfiguration
    {
        public IWardenSpawnConfigurator Configurator { get; }
        public IConfigurationReader<IWardenSpawnConfiguration> ConfigurationReader { get; }
        public string Configuration { get; }

        public WardenSpawnFactoryConfiguration(IConfigurationReader<IWardenSpawnConfiguration> configurationReader, 
            IWardenSpawnConfigurator configurator, 
            string configuration)
        {
            ConfigurationReader = configurationReader;
            Configurator = configurator;
            Configuration = configuration;
        }
    }
}