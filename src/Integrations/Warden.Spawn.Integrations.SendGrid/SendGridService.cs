using System;
using System.Linq;
using System.Threading.Tasks;
using Warden.Integrations.SendGrid;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridService : ISendGridService
    {
        private readonly SendGridIntegration _integration;
        private readonly SendGridSpawnIntegrationConfiguration _integrationConfiguration;

        public SendGridService(SendGridIntegration integration, 
            SendGridSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integration = integration;
            _integrationConfiguration = integrationConfiguration;
        }

        public async Task SendMessageAsync(object configuration)
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

            await _integration.SendEmailAsync(subject, message, receivers.ToArray());
        }
    }
}