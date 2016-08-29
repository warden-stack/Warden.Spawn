using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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

        public async Task<object> GetConfigurationAsync(string id, string token)
        {
            var response = await _httpClient.GetAsync($"wardenconfigurations/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(content);
        }
    }
}