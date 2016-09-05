namespace Warden.Spawn.Hooks
{
    public class WatcherHookSpawnConfiguration : IWatcherHookSpawnConfiguration
    {
        public WatcherHookType Type { get; set; }
        public HookCondition Condition { get; set; }
        public string Use { get; set; }
        public object Configuration { get; set; }
    }
}