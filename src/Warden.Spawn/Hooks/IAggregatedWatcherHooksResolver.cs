using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Warden.Spawn.Hooks
{
    public interface IAggregatedWatcherHooksResolver
    {
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess(object configuration, HookCondition condition);

        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync(object configuration,
            HookCondition condition);

        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess(object configuration, HookCondition condition);

        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync(object configuration,
            HookCondition condition);

        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure(object configuration, HookCondition condition);

        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync(object configuration,
            HookCondition condition);

        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstFailure(object configuration, HookCondition condition);

        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstFailureAsync(object configuration,
            HookCondition condition);

        Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration, HookCondition condition);

        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration,
            HookCondition condition);

        Expression<Action<IEnumerable<Exception>>> OnError(object configuration);
        Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync(object configuration);
        Expression<Action<IEnumerable<Exception>>> OnFirstError(object configuration);
        Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync(object configuration);
    }
}