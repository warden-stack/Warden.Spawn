using Warden.Spawn.Core;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Url { get; }
    }
}