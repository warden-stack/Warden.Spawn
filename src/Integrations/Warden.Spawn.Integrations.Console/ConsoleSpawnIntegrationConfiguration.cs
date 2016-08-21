using System.Collections.Generic;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationConfiguration : ISpawnIntegrationConfiguration
    {
        public string DefaultText { get; set; }

        [CredentialValue]
        public string Password { get; set; }

        public IEnumerable<Credential> Credentials { get; }
    }
}