using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public interface IWatcherSpawnConfigurator<in T> : IConfigurator where T : IWatcherSpawnConfiguration
    {
        IWatcher Configure(T configuration);
    }
}