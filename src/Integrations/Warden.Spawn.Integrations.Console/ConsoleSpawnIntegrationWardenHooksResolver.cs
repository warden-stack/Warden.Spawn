using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationWardenHooksResolver : IWardenHooksResolver
    {
        private readonly IConsoleService _service;

        public ConsoleSpawnIntegrationWardenHooksResolver(IConsoleService service)
        {
            _service = service;
        }

        public Expression<Action> OnStart(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Func<Task>> OnStartAsync(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Action> OnPause(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Func<Task>> OnPauseAsync(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Action> OnStop(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Func<Task>> OnStopAsync(object configuration)
            => () => _service.PrintAsync(configuration);

        public Expression<Action<Exception>> OnError(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<Exception, Task>> OnErrorAsync(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IWardenIteration>> OnIterationCompleted(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IWardenIteration, Task>> OnIterationCompletedAsync(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<long>> OnIterationStart(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<long, Task>> OnIterationStartAsync(object configuration)
            => x => _service.PrintAsync(configuration);
    }
}