using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegration : ISpawnIntegration
    {
        public string Name => "Console";
        public IWatcherHooksResolver WatcherHooksResolver { get; }

        public ConsoleSpawnIntegration(ConsoleSpawnIntegrationConfiguration configuration)
        {
            WatcherHooksResolver = new ConsoleSpawnIntegrationWatcherHooksResolver(configuration);
        }
    }
}