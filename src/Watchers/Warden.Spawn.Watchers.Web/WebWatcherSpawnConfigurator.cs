using System.Linq;
using Warden.Spawn.Configurations;
using Warden.Watchers;
using Warden.Watchers.Web;

namespace Warden.Spawn.Watchers.Web
{
    public class WebWatcherSpawnConfigurator : IWatcherSpawnConfigurator<WebWatcherSpawnConfiguration>
    {
        public IWatcher Configure(WebWatcherSpawnConfiguration configuration)
        {
            var request = GetRequest(configuration);
            var watcherConfiguration = WebWatcherConfiguration
                .Create(configuration.Url, request)
                .WithTimeout(configuration.Timeout);

            if (configuration.SkipStatusCodeValidation)
                watcherConfiguration.SkipStatusCodeValidation();

            return WebWatcher.Create(watcherConfiguration.Build());
        }

        private IHttpRequest GetRequest(WebWatcherSpawnConfiguration configuration)
        {
            if (configuration.Request == null)
                return null;

            var endpoint = configuration.Request.Endpoint;
            var headers = configuration.Request.Headers?.ToDictionary(x => x.Name, x => x.Value);
            var data = configuration.Request.Data;
            switch (configuration.Request.Method)
            {
                case HttpMethod.Get:
                    return HttpRequest.Get(endpoint, headers);
                case HttpMethod.Post:
                    return HttpRequest.Post(endpoint, data, headers);
                case HttpMethod.Put:
                    return HttpRequest.Put(endpoint, data, headers);
                case HttpMethod.Delete:
                    return HttpRequest.Delete(endpoint, headers);
            }

            return null;
        }
    }
}