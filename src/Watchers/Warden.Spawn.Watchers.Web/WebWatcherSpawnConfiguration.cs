using System;
using Warden.Spawn.Configurations;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfiguration : IWatcherSpawnConfiguration
    {
        public string Name => "Web";
        public string Url { get; set; }
        public HttpRequestDefinition Request { get; set; }
        public TimeSpan Timeout { get; set; }
        public bool SkipStatusCodeValidation { get; set; }
    }
}