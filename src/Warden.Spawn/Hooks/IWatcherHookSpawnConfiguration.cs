namespace Warden.Spawn.Hooks
{
    public interface IWatcherHookSpawnConfiguration
    {
        WatcherHookType Type { get; }
        Condition Condition { get; }
        string Use { get; }
        object Configuration { get; set; }
    }
}