using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationAggregatedWatcherHooksResolver : IAggregatedWatcherHooksResolver
    {
        private readonly ISendGridService _service;

        public SendGridIntegrationAggregatedWatcherHooksResolver(ISendGridService service)
        {
            _service = service;
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstFailure(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstFailureAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<Exception>>> OnError(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Action<IEnumerable<Exception>>> OnFirstError(object configuration)
            => x => _service.SendMessageAsync(configuration);

        public Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync(object configuration)
            => x => _service.SendMessageAsync(configuration);
    }
}