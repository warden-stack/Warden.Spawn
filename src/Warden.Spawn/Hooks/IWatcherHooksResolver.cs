using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Watchers;

namespace Warden.Spawn.Hooks
{
    public interface IWatcherHooksResolver
    {
        Expression<Action<IWatcherCheck>> OnStart();
        Expression<Func<IWatcherCheck, Task>> OnStartAsync();
        Expression<Action<IWardenCheckResult>> OnSuccess();
        Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync();
        Expression<Action<IWardenCheckResult>> OnFirstSuccess();
        Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync();
        Expression<Action<IWardenCheckResult>> OnFailure();
        Expression<Func<IWardenCheckResult, Task>> OnFailureAsync();
        Expression<Action<IWardenCheckResult>> OnCompleted(object configuration);
        Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration);
        Expression<Action<Exception>> OnError();
        Expression<Func<Exception, Task>> OnErrorAsync();
        Expression<Action<Exception>> OnFirstError();
        Expression<Func<Exception, Task>> OnFirstErrorAsync();
    }
}