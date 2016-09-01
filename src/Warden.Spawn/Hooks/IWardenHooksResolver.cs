using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Warden.Spawn.Hooks
{
    public interface IWardenHooksResolver
    {
        Expression<Action<Exception>> OnError(object configuration);
        Expression<Func<Exception, Task>> OnErrorAsync(object configuration);
        Expression<Action<IWardenIteration>> OnIterationCompleted(object configuration);
        Expression<Func<IWardenIteration, Task>> OnIterationCompletedAsync(object configuration);
        Expression<Action<long>> OnIterationStart(object configuration);
        Expression<Func<long, Task>> OnIterationStartAsync(object configuration);
        Expression<Action> OnPause(object configuration);
        Expression<Func<Task>> OnPauseAsync(object configuration);
        Expression<Action> OnStart(object configuration);
        Expression<Func<Task>> OnStartAsync(object configuration);
        Expression<Action> OnStop(object configuration);
        Expression<Func<Task>> OnStopAsync(object configuration);
    }
}