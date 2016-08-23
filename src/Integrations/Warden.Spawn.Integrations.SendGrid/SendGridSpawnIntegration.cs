using Warden.Integrations.SendGrid;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegration : ISpawnIntegration
    {
        private readonly SendGridSpawnIntegrationConfiguration _configuration;
        public string Name => "SendGrid";
        public IWatcherHooksResolver WatcherHooksResolver { get; protected set; }
        public IWardenHooksResolver WardenHooksResolver { get; }
        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public SendGridSpawnIntegration(SendGridSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
            var sendGridConfiguration = SendGridIntegrationConfiguration
                .Create(configuration.ApiKey, configuration.Sender)
                .Build();

            var integration = SendGridIntegration.Create(sendGridConfiguration);
            WatcherHooksResolver = new SendGridIntegrationWatcherHooksResolver(integration, configuration);
            WardenHooksResolver = new SendGridIntegrationWardenHooksResolver(integration, configuration);
        }
    }
}