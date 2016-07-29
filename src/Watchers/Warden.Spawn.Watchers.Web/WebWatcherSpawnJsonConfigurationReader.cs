using Warden.Spawn.Core;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnJsonConfigurationReader : IConfigurationReader<WebWatcherSpawnConfiguration>
    {
        public WebWatcherSpawnConfiguration Read(string configuration)
        {
            return new WebWatcherSpawnConfiguration("google.com");
        }
    }
}