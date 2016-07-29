namespace Warden.Spawn.Core
{
    public interface IWardenSpawn
    {
        IWarden Spawn(WardenSpawnConfiguration configuration);
    }
}