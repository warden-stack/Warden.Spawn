using Warden.Integrations;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegration : ISpawnIntegration
    {
        private ISendGridService _service;
        private readonly SendGridSpawnIntegrationConfiguration _configuration;
        public string Name => "SendGrid";
        public IIntegration Integration { get; set; }

        public IWatcherHooksResolver WatcherHooksResolver
            => new SendGridIntegrationWatcherHooksResolver(Service);

        public IWardenHooksResolver WardenHooksResolver
            => new SendGridIntegrationWardenHooksResolver(Service);

        public IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver
            => new SendGridIntegrationAggregatedWatcherHooksResolver(Service);

        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public SendGridSpawnIntegration(SendGridSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ISendGridService Service
            => _service ?? (_service = new SendGridService(Integration as SendGridIntegration, _configuration));
    }
}