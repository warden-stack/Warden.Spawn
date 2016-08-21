using System.Collections.Generic;

namespace Warden.Spawn.Configurations
{
    public interface ISpawnIntegrationConfiguration : IConfiguration
    {
        IEnumerable<Credential> Credentials { get; }
    }
}