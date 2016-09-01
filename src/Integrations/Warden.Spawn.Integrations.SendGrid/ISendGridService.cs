using System.Threading.Tasks;

namespace Warden.Spawn.Integrations.SendGrid
{
    public interface ISendGridService
    {
        Task SendMessageAsync(object configuration);
    }
}