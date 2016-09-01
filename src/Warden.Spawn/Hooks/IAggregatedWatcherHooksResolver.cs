using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Watchers;

namespace Warden.Spawn.Hooks
{
    public interface IAggregatedWatcherHooksResolver
    {
        Expression<Action<IEnumerable<IWatcherCheck>>> OnStart();
        Expression<Func<IEnumerable<IWatcherCheck>, Task>> OnStartAsync();
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess();
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync();
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess();
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync();
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure();
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync();
        Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration);
        Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration);
        Expression<Action<IEnumerable<Exception>>> OnError();
        Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync();
        Expression<Action<IEnumerable<Exception>>> OnFirstError();
        Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync();
    }
}