using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Warden.Spawn.Hooks
{
    public interface IWardenHooksResolver
    {
        Expression<Action<IWardenIteration>> OnIterationCompleted(object configuration);
        Expression<Func<IWardenIteration, Task>> OnIterationCompletedAsync(object configuration);
    }
}