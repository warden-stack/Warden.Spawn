using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationAggregatedWatcherHooksResolver : IAggregatedWatcherHooksResolver
    {
        private readonly IConsoleService _service;

        public ConsoleSpawnIntegrationAggregatedWatcherHooksResolver(IConsoleService service)
        {
            _service = service;
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstFailure(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstFailureAsync(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration,
                HookCondition condition)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<Exception>>> OnError(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Action<IEnumerable<Exception>>> OnFirstError(object configuration)
            => x => _service.PrintAsync(configuration);

        public Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync(object configuration)
            => x => _service.PrintAsync(configuration);
    }
}