using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public interface IWatcherSpawnConfigurator<in T>
    {
        IWatcher Configure(T configuration);
    }
}