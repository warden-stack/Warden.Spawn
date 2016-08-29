using System.Threading.Tasks;

namespace Warden.Spawn.Tools.Core.Services
{
    public interface IWardenConfigurationManager
    {
        Task<object> GetConfigurationAsync(string id, string token);
    }
}