using Warden.Watchers;

namespace Warden.Spawn.Core
{
    public interface IWatcherConfigurator<in T> where T : IWatcherConfiguration
    {
        IWatcher Configure(T configuration);
    }
}