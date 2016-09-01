using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationAggregatedWatcherHooksResolver : IAggregatedWatcherHooksResolver
    {
        private readonly SendGridIntegration _integration;
        private readonly SendGridSpawnIntegrationConfiguration _integrationConfiguration;

        public SendGridIntegrationAggregatedWatcherHooksResolver(SendGridIntegration integration,
            SendGridSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integration = integration;
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IEnumerable<IWatcherCheck>>> OnStart()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWatcherCheck>, Task>> OnStartAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration)
        {
            var config = configuration as SendGridSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var subject = string.IsNullOrWhiteSpace(config.Subject)
                ? _integrationConfiguration.DefaultSubject
                : config.Subject;
            var message = string.IsNullOrWhiteSpace(config.Message)
                ? _integrationConfiguration.DefaultMessage
                : config.Message;
            var receivers = config.Receivers == null || !config.Receivers.Any()
                ? _integrationConfiguration.DefaultReceivers
                : config.Receivers;

            return x => Task.Factory.StartNew(() => _integration.SendEmailAsync(subject, message, receivers.ToArray()));
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration)
        {
            var config = configuration as SendGridSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var subject = string.IsNullOrWhiteSpace(config.Subject)
                ? _integrationConfiguration.DefaultSubject
                : config.Subject;
            var message = string.IsNullOrWhiteSpace(config.Message)
                ? _integrationConfiguration.DefaultMessage
                : config.Message;
            var receivers = config.Receivers == null || !config.Receivers.Any()
                ? _integrationConfiguration.DefaultReceivers
                : config.Receivers;

            return x => _integration.SendEmailAsync(subject, message, receivers.ToArray());
        }

        public Expression<Action<IEnumerable<Exception>>> OnError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<Exception>>> OnFirstError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync()
        {
            throw new NotImplementedException();
        }
    }
}