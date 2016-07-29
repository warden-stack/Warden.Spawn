namespace Warden.Spawn.Core
{
    public interface IWardenSpawnConfigurator
    {
        IWardenSpawn Configure(IWardenSpawnConfiguration configuration);
    }
}