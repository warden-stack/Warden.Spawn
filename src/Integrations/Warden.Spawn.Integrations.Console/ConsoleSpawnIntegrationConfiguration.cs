using Warden.Spawn.Configurations;
using Warden.Spawn.Security;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationConfiguration : ISpawnIntegrationConfiguration
    {
        public string DefaultText { get; set; }

        [Credential]
        public string Password { get; set; }
    }
}