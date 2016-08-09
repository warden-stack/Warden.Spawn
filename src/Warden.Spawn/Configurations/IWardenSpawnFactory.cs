namespace Warden.Spawn.Configurations
{
    public interface IWardenSpawnFactory
    {
        IWardenSpawn Resolve();
    }
}