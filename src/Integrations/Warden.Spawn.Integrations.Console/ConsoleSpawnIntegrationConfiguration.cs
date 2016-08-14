using Warden.Spawn.Configurations;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationConfiguration : ISpawnIntegrationConfiguration
    {
        public string DefaultText { get; set; }
    }
}