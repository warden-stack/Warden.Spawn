using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Watchers;

namespace Warden.Spawn.Hooks
{
    public interface IWatcherHooksResolver
    {
        Expression<Action<IWatcherCheck>> OnStart(object configuration);
        Expression<Func<IWatcherCheck, Task>> OnStartAsync(object configuration);
        Expression<Action<IWardenCheckResult>> OnSuccess(object configuration);
        Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync(object configuration);
        Expression<Action<IWardenCheckResult>> OnFirstSuccess(object configuration);
        Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync(object configuration);
        Expression<Action<IWardenCheckResult>> OnFailure(object configuration);
        Expression<Func<IWardenCheckResult, Task>> OnFailureAsync(object configuration);
        Expression<Action<IWardenCheckResult>> OnCompleted(object configuration);
        Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration);
        Expression<Action<Exception>> OnError(object configuration);
        Expression<Func<Exception, Task>> OnErrorAsync(object configuration);
        Expression<Action<Exception>> OnFirstError(object configuration);
        Expression<Func<Exception, Task>> OnFirstErrorAsync(object configuration);
    }
}