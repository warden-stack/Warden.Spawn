using System;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Name => "Web";
        public string Url { get; protected set; }
        public Type ConfiguratorType => typeof(WebWatcherSpawnConfigurator);

        protected WebWatcherSpawnConfiguration()
        {
        }

        public WebWatcherSpawnConfiguration(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL can not be empty.", nameof(url));

            Url = url;
        }

    }
}