﻿using System;
using Warden.Spawn.Core;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Name => "Web";
        public static IConfigurationType Type => new ConfigurationType(typeof(WebWatcherSpawnConfiguration), "Web");
        public string Url { get; protected set; }

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