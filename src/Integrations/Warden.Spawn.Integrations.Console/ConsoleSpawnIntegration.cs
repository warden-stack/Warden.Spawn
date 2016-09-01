using Warden.Integrations;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegration : ISpawnIntegration
    {
        private readonly ConsoleSpawnIntegrationConfiguration _configuration;
        public string Name => "Console";
        public IIntegration Integration { get; set; }

        public IWatcherHooksResolver WatcherHooksResolver
            => new ConsoleSpawnIntegrationWatcherHooksResolver(_configuration);

        public IWardenHooksResolver WardenHooksResolver
            => new ConsoleSpawnIntegrationWardenHooksResolver(_configuration);

        public IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver
            => new ConsoleSpawnIntegrationAggregatedWatcherHooksResolver(_configuration);

        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public ConsoleSpawnIntegration(ConsoleSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}