using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public interface IWatcherSpawnConfigurator<in T> : IConfigurator<T, IWatcher> where T : IWatcherSpawnConfiguration
    {
    }
}