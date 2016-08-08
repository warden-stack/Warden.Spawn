namespace Warden.Spawn.Hooks
{
    public interface IWatcherHookSpawnConfiguration
    {
        WatcherHookType Type { get; }
        Condition Condition { get; }
        string Action { get; }
        object Configuration { get; }
    }
}