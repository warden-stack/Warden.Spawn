using Warden.Core;

namespace Warden.Spawn.Core
{
    public class WardenSpawn : IWardenSpawn
    {
        public IWarden Spawn(IWardenSpawnConfiguration configuration)
        {
            var wardenConfiguration = WardenConfiguration
                .Create()
                .Build();

            return WardenInstance.Create(configuration.WardenName, wardenConfiguration);
        }
    }
}