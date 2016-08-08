namespace Warden.Spawn.Configurations
{
    public class WardenSpawnFactory : IWardenSpawnFactory
    {
        private readonly IWardenSpawnFactoryConfiguration _configuration;

        public WardenSpawnFactory(IWardenSpawnFactoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IWardenSpawn Create()
        {
            var spawnConfiguration = _configuration.ConfigurationReader.Read(_configuration.Configuration);
            var spawn = _configuration.Configurator.Configure(spawnConfiguration);

            return spawn;
        }
    }
}