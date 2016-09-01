using Warden.Integrations;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegration : ISpawnIntegration
    {
        private IConsoleService _service;
        private readonly ConsoleSpawnIntegrationConfiguration _configuration;
        public string Name => "Console";
        public IIntegration Integration { get; set; }

        public IWatcherHooksResolver WatcherHooksResolver
            => new ConsoleSpawnIntegrationWatcherHooksResolver(Service);

        public IWardenHooksResolver WardenHooksResolver
            => new ConsoleSpawnIntegrationWardenHooksResolver(Service);

        public IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver
            => new ConsoleSpawnIntegrationAggregatedWatcherHooksResolver(Service);

        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public ConsoleSpawnIntegration(ConsoleSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConsoleService Service
            => _service ?? (_service = new ConsoleService(_configuration));
    }
}