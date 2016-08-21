using System;
using System.Collections.Generic;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Name => "Web";
        public string Url { get; protected set; }
        public TimeSpan Timeout { get; set; }
        public IEnumerable<Credential> Credentials { get; }

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