using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Watchers;

namespace Warden.Spawn.Hooks
{
    public interface IAggregatedWatcherHooksResolver
    {
        Expression<Action<IEnumerable<IWatcherCheck>>> OnStart(object configuration);
        Expression<Func<IEnumerable<IWatcherCheck>, Task>> OnStartAsync(object configuration);
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess(object configuration);
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync(object configuration);
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess(object configuration);
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync(object configuration);
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure(object configuration);
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync(object configuration);
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration);
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration);
        Expression<Action<IEnumerable<Exception>>> OnError(object configuration);
        Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync(object configuration);
        Expression<Action<IEnumerable<Exception>>> OnFirstError(object configuration);
        Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync(object configuration);
    }
}