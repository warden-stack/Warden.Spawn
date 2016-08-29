using System.Threading.Tasks;

namespace Warden.Spawn.Tools.Core.Services
{
    public interface IWardenConfigurationManager
    {
        Task<string> GetConfigurationAsync(string id, string token);
    }
}