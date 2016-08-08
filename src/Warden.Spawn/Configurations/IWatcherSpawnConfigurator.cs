using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherSpawnConfigurator<in T> : IConfigurator<T, IWatcher> where T : IWatcherSpawnConfiguration
    {
    }
}