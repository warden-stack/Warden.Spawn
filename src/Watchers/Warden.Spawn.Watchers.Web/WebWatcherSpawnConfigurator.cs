using Warden.Spawn.Configurations;
using Warden.Watchers;
using Warden.Watchers.Web;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfigurator : IWatcherSpawnConfigurator<WebWatcherSpawnConfiguration>
    {
        public static IConfiguratorType Type => new ConfiguratorType(typeof(WebWatcherSpawnConfigurator), "Web");

        public IWatcher Configure(WebWatcherSpawnConfiguration configuration)
        {
            var watcherConfiguration = WebWatcherConfiguration
                .Create(configuration.Url)
                .WithTimeout(configuration.Timeout)
                .Build();

            return WebWatcher.Create(watcherConfiguration);
        }
    }
}