using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegration : ISpawnIntegration
    {
        private readonly ConsoleSpawnIntegrationConfiguration _configuration;
        public string Name => "Console";
        public IWatcherHooksResolver WatcherHooksResolver { get; }
        public IWardenHooksResolver WardenHooksResolver { get; }
        public IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver { get; }
        public ISpawnIntegrationConfiguration Configuration => _configuration;

        public ConsoleSpawnIntegration(ConsoleSpawnIntegrationConfiguration configuration)
        {
            _configuration = configuration;
            WatcherHooksResolver = new ConsoleSpawnIntegrationWatcherHooksResolver(configuration);
            WardenHooksResolver = new ConsoleSpawnIntegrationWardenHooksResolver(configuration);
            AggregatedWatcherHooksResolver = new ConsoleSpawnIntegrationAggregatedWatcherHooksResolver(configuration);
        }
    }
}