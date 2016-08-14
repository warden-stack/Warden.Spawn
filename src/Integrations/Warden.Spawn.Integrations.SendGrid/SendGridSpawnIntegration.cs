using Warden.Integrations.SendGrid;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegration : ISpawnIntegration
    {
        public string Name => "SendGrid";
        public IWatcherHooksResolver WatcherHooksResolver { get; protected set; }

        public SendGridSpawnIntegration(SendGridSpawnIntegrationConfiguration configuration)
        {
            var sendGridConfiguration = SendGridIntegrationConfiguration
                .Create(configuration.ApiKey, configuration.Sender)
                .Build();

            var integration = SendGridIntegration.Create(sendGridConfiguration);

            WatcherHooksResolver = new SendGridIntegrationWatcherHooksResolver(integration, configuration);
        }
    }
}