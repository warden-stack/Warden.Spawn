namespace Warden.Spawn.Configurations
{
    public class WardenSpawnFactoryConfiguration : IWardenSpawnFactoryConfiguration
    {
        public IWardenSpawnConfigurator Configurator { get; protected set; }
        public IConfigurationReader<IWardenSpawnConfiguration> ConfigurationReader { get; protected set; }
        public string Configuration { get; protected set; }

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