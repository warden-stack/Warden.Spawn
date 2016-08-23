using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationWardenHooksResolver : IWardenHooksResolver
    {
        private readonly SendGridIntegration _integration;
        private readonly SendGridSpawnIntegrationConfiguration _integrationConfiguration;

        public SendGridIntegrationWardenHooksResolver(SendGridIntegration integration, 
            SendGridSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integration = integration;
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IWardenIteration>> OnIterationCompleted(object configuration)
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

        public Expression<Func<IWardenIteration, Task>> OnIterationCompletedAsync(object configuration)
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
    }
}