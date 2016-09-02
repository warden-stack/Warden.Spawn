using System;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherWithHooks
    {
        IWatcher Watcher { get; }
        Action<WatcherHooksConfiguration.Builder> Hooks { get; }
    }
}