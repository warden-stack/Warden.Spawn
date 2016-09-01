using Warden.Integrations;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegration : ISpawnIntegration
    {
        private readonly SendGridSpawnIntegrationConfiguration _configuration;
        public string Name => "SendGrid";
        public IIntegration Integration { get; set; }

        public IWatcherHooksResolver WatcherHooksResolver
            => new SendGridIntegrationWatcherHooksResolver(GetIntegration(), _configuration);

        public IWardenHooksResolver WardenHooksResolver
            => new SendGridIntegrationWardenHooksResolver(GetIntegration(), _configuration);

        public IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver
            => new SendGridIntegrationAggregatedWatcherHooksResolver(GetIntegration(), _configuration);

        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public SendGridSpawnIntegration(SendGridSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SendGridIntegration GetIntegration() => Integration as SendGridIntegration;
    }
}