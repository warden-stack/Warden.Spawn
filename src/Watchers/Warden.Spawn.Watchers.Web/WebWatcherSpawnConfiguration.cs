using System;
using Warden.Spawn.Core;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Url { get; }

        public WebWatcherSpawnConfiguration(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL can not be empty.", nameof(url));

            Url = url;
        }
    }
}