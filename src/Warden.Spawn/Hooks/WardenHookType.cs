namespace Warden.Spawn.Hooks
{
    public enum WardenHookType
    {
        Unknown = 0,
        OnStart,
        OnStartAsync,
        OnPause,
        OnPauseAsync,
        OnStop,
        OnStopAsync,
        OnError,
        OnErrorAsync,
        OnIterationStart,
        OnIterationStartAsync,
        OnIterationCompleted,
        OnIterationCompletedAsync
    }
}