using System.Threading.Tasks;

namespace Warden.Spawn.Integrations.Console
{
    public interface IConsoleService
    {
        Task PrintAsync(object configuration);
    }
}