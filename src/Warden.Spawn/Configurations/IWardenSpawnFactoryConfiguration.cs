namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnFactoryConfiguration
    {
        IWardenSpawnConfigurator Configurator { get; }
        IConfigurationReader<IWardenSpawnConfiguration> ConfigurationReader { get; }
        string Configuration { get; }
    }
}