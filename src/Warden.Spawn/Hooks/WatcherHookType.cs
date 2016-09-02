namespace Warden.Spawn.Hooks
{
    public enum WatcherHookType
    {
        Unknown = 0,
        OnStart,
        OnStartAsync,
        OnSuccess,
        OnSuccessAsync,
        OnFirstSuccess,
        OnFirstSuccessAsync,
        OnFailure,
        OnFailureAsync,
        OnFirstFailure,
        OnFirstFailureAsync,
        OnCompleted,
        OnCompletedAsync,
        OnError,
        OnErrorAsync,
        OnFirstError,
        OnFirstErrorAsync
    }
}