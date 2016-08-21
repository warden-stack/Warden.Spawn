using System.Collections.Generic;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawnConfiguration : IConfiguration
    {
        string Name { get; }
        IEnumerable<Credential> Credentials { get; }
    }
}