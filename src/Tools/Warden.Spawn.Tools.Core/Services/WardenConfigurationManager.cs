using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace Warden.Spawn.Tools.Core.Services
{
    public class WardenConfigurationManager : IWardenConfigurationManager
    {
        private readonly HttpClient _httpClient;

        public WardenConfigurationManager(string apiUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            };
        }

        public async Task<string> GetConfigurationAsync(string id, string token)
        {
            var configuration = await _httpClient.GetAsync($"/wardens/configurations/{id}?token={token}");

            return await configuration.Content.ReadAsStringAsync();
        }
    }
}