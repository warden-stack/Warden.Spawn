using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationWatcherHooksResolver : IWatcherHooksResolver
    {
        private readonly ISendGridService _service;

        public SendGridIntegrationWatcherHooksResolver(ISendGridService service)
        {
            _service = service;
        }

        public Expression<Action<IWatcherCheck>> OnStart(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWatcherCheck, Task>> OnStartAsync(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IWardenCheckResult>> OnSuccess(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IWardenCheckResult>> OnFirstSuccess(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync(object configuration,
                HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IWardenCheckResult>> OnFailure(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWardenCheckResult, Task>> OnFailureAsync(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IWardenCheckResult>> OnFirstFailure(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWardenCheckResult, Task>> OnFirstFailureAsync(object configuration,
                HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IWardenCheckResult>> OnCompleted(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration, HookCondition condition)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<Exception>> OnError(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<Exception, Task>> OnErrorAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<Exception>> OnFirstError(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<Exception, Task>> OnFirstErrorAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);
    }
}