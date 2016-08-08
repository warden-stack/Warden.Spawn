﻿namespace Warden.Spawn.Hooks
{
    public class WatcherHookSpawnConfiguration : IWatcherHookSpawnConfiguration
    {
        public WatcherHookType Type { get; set; }
        public Condition Condition { get; set; }
        public string Action { get; set; }
        public object Configuration { get; set; }
    }
}