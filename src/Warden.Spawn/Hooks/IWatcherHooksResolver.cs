using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Watchers;

namespace Warden.Spawn.Hooks
{
    public interface IWatcherHooksResolver
    {
        Expression<Action<IWatcherCheck>> OnStart(object configuration, HookCondition condition);
        Expression<Func<IWatcherCheck, Task>> OnStartAsync(object configuration, HookCondition condition);
        Expression<Action<IWardenCheckResult>> OnSuccess(object configuration, HookCondition condition);
        Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync(object configuration, HookCondition condition);
        Expression<Action<IWardenCheckResult>> OnFirstSuccess(object configuration, HookCondition condition);
        Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync(object configuration, HookCondition condition);
        Expression<Action<IWardenCheckResult>> OnFailure(object configuration, HookCondition condition);
        Expression<Func<IWardenCheckResult, Task>> OnFailureAsync(object configuration, HookCondition condition);
        Expression<Action<IWardenCheckResult>> OnFirstFailure(object configuration, HookCondition condition);
        Expression<Func<IWardenCheckResult, Task>> OnFirstFailureAsync(object configuration, HookCondition condition);
        Expression<Action<IWardenCheckResult>> OnCompleted(object configuration, HookCondition condition);
        Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration, HookCondition condition);
        Expression<Action<Exception>> OnError(object configuration);
        Expression<Func<Exception, Task>> OnErrorAsync(object configuration);
        Expression<Action<Exception>> OnFirstError(object configuration);
        Expression<Func<Exception, Task>> OnFirstErrorAsync(object configuration);
    }
}