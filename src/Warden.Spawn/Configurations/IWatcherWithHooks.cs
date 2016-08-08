using System;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public interface IWatcherWithHooks
    {
        IWatcher Watcher { get; }
        Action<WatcherHooksConfiguration.Builder> Hooks { get; }
    }

    public class WatcherWithHooks : IWatcherWithHooks
    {
        public IWatcher Watcher { get; }
        public Action<WatcherHooksConfiguration.Builder> Hooks { get; }


        public WatcherWithHooks(IWatcher watcher, Action<WatcherHooksConfiguration.Builder> hooks)
        {
            Watcher = watcher;
            Hooks = hooks;
        }
    }
}