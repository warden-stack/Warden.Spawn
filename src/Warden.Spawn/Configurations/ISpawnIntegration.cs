using Warden.Integrations;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface ISpawnIntegration
    {
        string Name { get; }
        IIntegration Integration { get; set; }
        IWatcherHooksResolver WatcherHooksResolver { get; }
        IWardenHooksResolver WardenHooksResolver { get; }
        IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver { get; }
        ISpawnIntegrationConfiguration Configuration { get; }
    }
}