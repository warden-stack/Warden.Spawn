using Warden.Spawn.Core;
using Warden.Watchers;
using Warden.Watchers.Web;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfigurator : IWatcherSpawnConfigurator<WebWatcherSpawnConfiguration>
    {
        public IWatcher Configure(WebWatcherSpawnConfiguration configuration)
        {
            var watcherConfiguration = WebWatcherConfiguration
                .Create(configuration.Url)
                .Build();

            return WebWatcher.Create(watcherConfiguration);
        }
    }
}