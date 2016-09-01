using Warden.Integrations;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegrationConfigurator : IIntegrationSpawnConfigurator<SendGridSpawnIntegrationConfiguration>
    {
        public IIntegration Configure(SendGridSpawnIntegrationConfiguration configuration)
        {
            var integrationConfiguration = SendGridIntegrationConfiguration
                .Create(configuration.ApiKey, configuration.Sender)
                .Build();

            return SendGridIntegration.Create(integrationConfiguration);
        }
    }
}