using Warden.Spawn.Hooks;

namespace Warden.Spawn.Configurations
{
    public interface ISpawnIntegration
    {
        string Name { get; }
        IWatcherHooksResolver WatcherHooksResolver { get; }
        IWardenHooksResolver WardenHooksResolver { get; }
        IAggregatedWatcherHooksResolver AggregatedWatcherHooksResolver { get; }
        ISpawnIntegrationConfiguration Configuration { get; }
    }
}